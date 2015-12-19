﻿namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Contracts;

    using Data.Models;

    using Models;

    using Newtonsoft.Json;

    public class WebApiServerClient : IWebApiServerClient
    {
        private readonly IServersManager serversManager;

        public WebApiServerClient(IServersManager serversManager)
        {
            this.serversManager = serversManager;
        }
        
        public async Task<int> GetProductsCountAsync()
        {
            var response = await this.serversManager.FastestServer.GetAsync("api/products");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(result);
        }
        
        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize)
        {
            var response = await this.serversManager.FastestServer.GetAsync($"api/products?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<SimplifiedProduct>>(result);
        }

        public async Task<IDictionary<ServerInformation, Product>> GetProductByIdAsync(int id)
        {
            var products = new Dictionary<ServerInformation, Product>();

            foreach (var server in this.serversManager.ActiveServers)
            {
                var response = await server.Value.GetAsync($"api/products/{id}");
                var result = await response.Content.ReadAsStringAsync();

                products.Add(server.Key, JsonConvert.DeserializeObject<Product>(result));
            }

            return products;
        }

        public async void UpdateAsync(IDictionary<ServerInformation, Product> productsServersPair)
        {
            foreach (var pair in productsServersPair)
            {
                var client = this.serversManager.ActiveServers[pair.Key];

                var serialized = JsonConvert.SerializeObject(pair.Value);
                var content = new StringContent(serialized, Encoding.UTF8, "application/json");

                var response = await client.PutAsync("api/products", content);
                var result = await response.Content.ReadAsStringAsync();
            }
        }
    }
}