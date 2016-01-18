namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using WebApiModels;

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
            this.Cache.ReportsPerServer.Clear();

            var missingProducts = new ConcurrentDictionary<MissingProduct, byte>();
            await Task.Run(() =>
                     {
                         Parallel.ForEach(this.storage.Servers, (server) =>
                                                                      {
                                                                          var client = this.tester.TestServer(server).Result;
                                                                          server.Client = client;
                                                                          this.Cache.ProductsPerServer.Add(server, new ObservableCollection<Product>());
                                                                          this.Cache.ReportsPerServer.Add(server, new ObservableCollection<Report>());
                                                                          
                                                                          if (client != null)
                                                                          {
                                                                              foreach (var missingProduct in this.apiClient.GetMissingProductsAsync(client).Result)
                                                                              {
                                                                                  missingProducts.TryAdd(missingProduct, 1);
                                                                              }

                                                                              this.Cache.ProductsPerServer[server] = this.apiClient.GetProductsAsync(client).Result;

                                                                              this.Cache.ReportsPerServer[server] = this.apiClient.GetReportsAsync(client).Result;
                                                                          }
                                                                      });
                     });

            this.Cache.MissingProducts = missingProducts.Keys;

            this.Cache.SetMainProducts();

            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}