namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    public class ServerManager : IServerManager
    {
        private readonly IServerStorage storage;
        private readonly IServerTester tester;
        private readonly IWebApiClient apiClient;

        public ServerManager(IServerStorage storage, IServerTester tester, IWebApiClient apiClient, IProductsCache cache)
        {
            this.storage = storage;
            this.storage.Servers.CollectionChanged += (s, e) =>
                                                      {
                                                          this.Initialize();
                                                      };
            this.tester = tester;
            this.apiClient = apiClient;
            this.Cache = cache;

            this.Initialize();
        }

        public IProductsCache Cache { get; set; }

        public async Task AddOrUpdateAsync(ServerInformation server, AddProductModel model)
        {
            var currentClient = server.Client;
            if (currentClient == null)
            {
                //TODO: retry later
            }
            else
            {
                await this.apiClient.AddOrUpdateAsync(currentClient, model);
            }
        }

        public event EventHandler ServerTestingFinished;

        //TODO: add a way to cancel
        public async void Initialize()
        {
            this.Cache.ProductsPerServer.Clear();
            var missingProducts = new ConcurrentDictionary<MissingProduct, byte>();
            await Task.Run(() =>
                     {
                         Parallel.ForEach(this.storage.Servers, (server) =>
                                                                      {
                                                                          var client = this.tester.TestServer(server).Result;
                                                                          server.Client = client;
                                                                          this.Cache.ProductsPerServer.Add(server, new ObservableCollection<Product>());
                                                                          
                                                                          if (client != null)
                                                                          {
                                                                              foreach (var missingProduct in this.apiClient.GetMissingProductsAsync(client).Result)
                                                                              {
                                                                                  missingProducts.TryAdd(missingProduct, 1);
                                                                              }

                                                                              foreach (var product in this.apiClient.GetProductsAsync(client).Result)
                                                                              {
                                                                                  this.Cache.ProductsPerServer[server].Add(product);
                                                                              }
                                                                          }
                                                                      });
                     });

            foreach (var pair in missingProducts)
            {
                this.Cache.MissingProducts.Add(pair.Key);
            }

            var singleServerProducts = this.Cache.ProductsPerServer.FirstOrDefault(x => x.Value.Any()).Value;
            if (singleServerProducts != null)
            {
                this.Cache.Products = singleServerProducts;
            }

            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}