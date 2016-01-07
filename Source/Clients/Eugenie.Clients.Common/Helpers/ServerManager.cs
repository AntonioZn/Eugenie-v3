namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    public class ServerManager : IServerManager
    {
        private const int PageSize = 200;

        private readonly IServerStorage storage;
        private readonly IServerTester tester;
        private readonly IWebApiClient apiClient;

        public ServerManager(IServerStorage storage, IServerTester tester, IWebApiClient apiClient, IProductsCache cache)
        {
            this.storage = storage;
            //this.storage.ServerAdded += this.OnServerAdded;
            //this.storage.ServerDeleted += this.OnServerDeleted;
            this.tester = tester;
            this.apiClient = apiClient;
            this.Cache = cache;

            this.Initialize();
        }

        public IProductsCache Cache { get; set; }

        public async void Initialize()
        {
            this.Cache.ProductsPerServer.Clear();
            await Task.Run(() =>
                     {
                         Parallel.ForEach(this.storage.Servers, (server) =>
                                                                      {
                                                                          this.Cache.ProductsPerServer.Add(server, new ObservableCollection<Product>());
                                                                          var client = this.tester.TestServer(server).Result;
                                                                          if (client != null)
                                                                          {
                                                                              var products = this.GetProductsAsync(client).Result;
                                                                              foreach (var product in products)
                                                                              {
                                                                                  this.Cache.ProductsPerServer[server].Add(product);
                                                                              }
                                                                          }
                                                                      });
                     });

            this.Cache.Products = this.Cache.ProductsPerServer.FirstOrDefault().Value;
            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }

        public void AddOrUpdateAsync(IDictionary<ServerInformation, Product> serverProductPairs)
        {
            foreach (var pair in serverProductPairs)
            {
                //var currentClient = this.ActiveServers[pair.Key];
                //this.apiClient.AddOrUpdateAsync(currentClient, pair.Value);
            }
        }

        public event EventHandler ServerTestingFinished;

        private async Task<IEnumerable<Product>> GetProductsAsync(HttpClient client)
        {
            var productCount = await this.apiClient.GetProductsCountAsync(client);
            var pageCount = (productCount + PageSize - 1) / PageSize;

            var result = new List<Product>(productCount);
            for (int i = 1; i <= pageCount; i++)
            {
                var response = await this.apiClient.GetProductsByPageAsync(client, i, PageSize);
                result.AddRange(response);
            }

            return result;
        }

        //private async void OnServerAdded(object sender, ServerAddedEventArgs e)
        //{
        //    this.Cache.ProductsPerServer.Add(e.Server, new ObservableCollection<Product>());
        //    var client = await this.tester.TestServer(e.Server);
        //    if (client != null)
        //    {
        //        var products = await this.GetProductsAsync(client);
        //        foreach (var product in products)
        //        {
        //            this.Cache.ProductsPerServer[e.Server].Add(product);
        //        }
        //    }
        //}
        //
        //private void OnServerDeleted(object sender, ServerDeletedEventArgs e)
        //{
        //    this.Cache.ProductsPerServer.Remove(e.Server);
        //}
    }
}