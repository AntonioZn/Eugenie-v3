namespace Eugenie.Clients.Admin.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Common.Helpers;
    using Common.Models;
    using Common.Еxtensions;

    using Contracts;

    using Models;

    using ViewModels;

    using Store = Models.Store;

    public class ServerManager : IServerManager
    {
        private readonly IServerStorage storage;
        private readonly ITaskManager taskManager;

        public ServerManager(IServerStorage storage, ITaskManager taskManager)
        {
            this.storage = storage;
            this.storage.Servers.CollectionChanged += (s, e) => this.Initialize();
            this.taskManager = taskManager;

            this.Initialize();
        }

        public event EventHandler SelectedServerChanged;

        public event EventHandler ServerTestingFinished;

        //TODO: Not necessary??
        public event EventHandler ProductsCacheChanged;

        public Store SelectedServer { get; private set; }

        public void SetSelectedServer(string name)
        {
            if (this.SelectedServer?.Name != name)
            {
                this.SelectedServer = this.storage.Servers.FirstOrDefault(x => x.Name == name);
                this.SelectedServerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // TODO: add a way to cancel
        public async void Initialize()
        {
            await this.storage.Servers.ProcessConcurrently(async (store, token) =>
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

                var serverName = pair.Key.Name;
                var model = pair.Value.GetModel();
                var task = new AddOrUpdateProductTask(serverName, model);
                this.taskManager.AddTask(task);

                if (this.Cache.ProductsPerServer[pair.Key].All(x => x.Name != pair.Value.Product.Name))
                {
                    cacheChanged = true;
                    this.Cache.ProductsPerServer[pair.Key].Add(pair.Value.Product);
                }
            }

            if (cacheChanged)
            {
                this.ProductsCacheChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Delete(string productName)
        {
            foreach (var server in this.storage.Servers)
            {
                this.taskManager.AddTask(new DeleteProductTask(server.Name, productName));
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