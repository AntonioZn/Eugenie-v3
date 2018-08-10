namespace Eugenie.Clients.Admin.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Common.Helpers;
    using Common.Еxtensions;

    using Models;

    using Newtonsoft.Json;

    using Properties;

    using ViewModels;

    public class ServerManager
    {
        private readonly Settings settings;
        private readonly TaskManager taskManager;

        public ServerManager(Settings settings, TaskManager taskManager)
        {
            this.settings = settings;
            this.taskManager = taskManager;

            this.Stores = JsonConvert.DeserializeObject<ObservableCollection<Store>>(settings.Stores);
            this.Stores.CollectionChanged += (s, e) =>
                                             {
                                                 settings.Stores = JsonConvert.SerializeObject(this.Stores);
                                                 settings.Save();
                                                 this.Initialize();
                                             };
            
            this.taskManager.Start(this.Stores);
        }

        public ObservableCollection<Store> Stores { get; }

        public event EventHandler SelectedStoreChanged;

        public event EventHandler ServerTestingFinished;

        //TODO: Not necessary??
        public event EventHandler ProductsCacheChanged;

        public Store SelectedStore { get; private set; }

        public void SetSelectedServer(string name)
        {
            if (this.SelectedStore?.Name != name)
            {
                this.SelectedStore = this.Stores.FirstOrDefault(x => x.Name == name);
                this.SelectedStoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // TODO: add a way to cancel
        public async void Initialize()
        {
            this.Stores.ForEach(x =>
                                {
                                    x.Client?.Dispose();
                                    x.Client = null;
                                });

            await this.Stores.ProcessConcurrently(async (store, token) =>
                                                           {
                                                               store.Products.Clear();
                                                               store.Reports.Clear();
                                                               store.MissingProducts.Clear();
                                                               store.Sellers.Clear();

                                                               var client = await this.GetClient(store);
                                                               store.Client = client;

                                                               if (client != null)
                                                               {
                                                                   store.Products = await client.GetProductsAsync();
                                                                   store.Reports = await client.GetReportsAsync();
                                                                   store.MissingProducts = await client.GetMissingProductsAsync();
                                                                   store.Sellers = await client.GetSellersAsync();
                                                               }
                                                           }, 2, CancellationToken.None);

            this.SetSelectedServer(string.Empty);
            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }

        public void AddOrUpdate(IDictionary<Store, ProductViewModel> productInAllServers, ProductViewModel mainProductViewModel)
        {
            var cacheChanged = false;
            foreach (var pair in productInAllServers)
            {
                pair.Value.MapProperties(mainProductViewModel);

                var serverId = pair.Key.Id;
                var model = pair.Value.GetModel();
                var task = new AddOrUpdateProductTask(serverId, model);
                this.taskManager.AddOrUpdateProductTasks.Add(task);

                if (pair.Key.Products.All(x => x.Name != pair.Value.Product.Name))
                {
                    cacheChanged = true;
                    pair.Key.Products.Add(pair.Value.Product);
                }
            }

            if (cacheChanged)
            {
                this.ProductsCacheChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Delete(string productName)
        {
            foreach (var server in this.Stores)
            {
                this.taskManager.DeleteProductTasks.Add(new DeleteProductTask(server.Id, productName));
                var product = server.Products.FirstOrDefault(x => x.Name == productName);
                if (product != null)
                {
                    server.Products.Remove(product);
                }
            }

            this.ProductsCacheChanged?.Invoke(this, EventArgs.Empty);
        }

        private async Task<StoreClient> GetClient(Store server)
        {
            var client = new StoreClient(server.Address);

            var isAuthenticated = await client.Authenticate(server.Username, server.Password);
            if (isAuthenticated)
            {
                return client;
            }

            return null;
        }
    }
}