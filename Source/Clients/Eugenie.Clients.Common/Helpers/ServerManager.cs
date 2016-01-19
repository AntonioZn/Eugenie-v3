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

        public ProductsCache Cache { get; }

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
            
            await Task.Run(() =>
                     {
                         Parallel.ForEach(this.storage.Servers, (server) =>
                                                                      {
                                                                          server.Client = ServerTester.TestServer(server).Result;
                                                                          this.Cache.ProductsPerServer.Add(server, new List<Product>());
                                                                          this.Cache.ReportsPerServer.Add(server, new List<Report>());
                                                                          this.Cache.MissingProductsPerServer.Add(server, new List<MissingProduct>());
                                                                          
                                                                          if (server.Client != null)
                                                                          {
                                                                              this.Cache.MissingProductsPerServer[server] = this.apiClient.GetMissingProductsAsync(server.Client).Result;

                                                                              this.Cache.ProductsPerServer[server] = this.apiClient.GetProductsAsync(server.Client).Result;

                                                                              this.Cache.ReportsPerServer[server] = this.apiClient.GetReportsAsync(server.Client).Result;
                                                                          }
                                                                      });
                     });
            
            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}