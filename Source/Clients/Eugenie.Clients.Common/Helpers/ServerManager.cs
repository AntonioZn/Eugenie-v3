namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using WebApiModels;

    public class ServerManager : IServerManager
    {
        private readonly IServerStorage storage;
        private readonly IWebApiClient apiClient;

        public ServerManager(IServerStorage storage, IWebApiClient apiClient)
        {
            this.storage = storage;
            this.storage.Servers.CollectionChanged += (s, e) =>
                                                      {
                                                          this.Initialize();
                                                      };
            this.apiClient = apiClient;
            this.Cache = new ProductsCache();

            this.Initialize();
        }

        public ProductsCache Cache { get; set; }

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
            this.Cache.MissingProductsPerServer.Clear();
            
            await Task.Run(() =>
                     {
                         Parallel.ForEach(this.storage.Servers, (server) =>
                                                                      {
                                                                          var client = ServerTester.TestServer(server).Result;
                                                                          server.Client = client;
                                                                          this.Cache.ProductsPerServer.Add(server, new List<Product>());
                                                                          this.Cache.ReportsPerServer.Add(server, new List<Report>());
                                                                          this.Cache.MissingProductsPerServer.Add(server, new List<MissingProduct>());
                                                                          
                                                                          if (client != null)
                                                                          {
                                                                              this.Cache.MissingProductsPerServer[server] = this.apiClient.GetMissingProductsAsync(client).Result;

                                                                              this.Cache.ProductsPerServer[server] = this.apiClient.GetProductsAsync(client).Result;

                                                                              this.Cache.ReportsPerServer[server] = this.apiClient.GetReportsAsync(client).Result;
                                                                          }
                                                                      });
                     });
            
            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}