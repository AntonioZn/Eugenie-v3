namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using WebApiModels;

    public class ServerManager : IServerManager
    {
        private readonly IServerStorage storage;
        private readonly IWebApiClient apiClient;
        private readonly IMessageQueueManager queueManager;

        public ServerManager(IServerStorage storage, IWebApiClient apiClient, IMessageQueueManager queueManager)
        {
            this.storage = storage;
            this.storage.Servers.CollectionChanged += (s, e) =>
                                                      {
                                                          this.Initialize();
                                                      };
            this.apiClient = apiClient;
            this.queueManager = queueManager;
            this.Cache = new Cache();

            this.Initialize();
        }

        public Cache Cache { get; }

        public void AddOrUpdate(ServerInformation server, AddProductModel model)
        {
            var pair = new ServerAddProductPair(server, model);
            this.queueManager.MessageQueue.Send(pair);
        }

        public event EventHandler SelectedServerChanged;

        public ServerInformation SelectedServer { get; private set; }

        public void SetSelectedServer(string name)
        {
            if (this.SelectedServer?.Name != name)
            {
                this.SelectedServer = this.storage.Servers.FirstOrDefault(x => x.Name == name);
                this.SelectedServerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler ServerTestingFinished;

        //TODO: add a way to cancel
        public async void Initialize()
        {
            this.Cache.ProductsPerServer.Clear();
            this.Cache.ReportsPerServer.Clear();
            this.Cache.MissingProductsPerServer.Clear();
            this.Cache.SellersPerServer.Clear();

            await Task.Run(() =>
            {
                Parallel.ForEach(this.storage.Servers, (server) =>
                                                                      {
                                                                          server.Client = ServerTester.TestServerAsync(server).Result;
                                                                          this.Cache.ProductsPerServer.Add(server, new List<Product>());
                                                                          this.Cache.ReportsPerServer.Add(server, new List<Report>());
                                                                          this.Cache.MissingProductsPerServer.Add(server, new List<MissingProduct>());
                                                                          this.Cache.SellersPerServer.Add(server, new List<Seller>());

                                                                          if (server.Client != null)
                                                                          {
                                                                              this.Cache.MissingProductsPerServer[server] = this.apiClient.GetMissingProductsAsync(server.Client).Result;
                                                                              this.Cache.ProductsPerServer[server] = this.apiClient.GetProductsAsync(server.Client).Result;
                                                                              this.Cache.ReportsPerServer[server] = this.apiClient.GetReportsAsync(server.Client).Result;
                                                                              this.Cache.SellersPerServer[server] = this.apiClient.GetSellersAsync(server.Client).Result;
                                                                          }
                                                                      });
            });

            this.SetSelectedServer("");
            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ProductsCacheChanged;

        public void AddProductToCache(Product product)
        {
            foreach (var pair in this.Cache.ProductsPerServer)
            {
                pair.Value.Add(product);
            }

            this.ProductsCacheChanged?.Invoke(this.Cache, EventArgs.Empty);
        }
    }
}