namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using Newtonsoft.Json;

    public class WebApiServerClient : IWebApiServerClient
    {
        public async Task<int> GetProductsCountAsync(KeyValuePair<ServerInformation, HttpClient> server)
        {
            var response = await server.Value.GetAsync("api/products");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(result);
        }
        
        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(KeyValuePair<ServerInformation, HttpClient> server, int page, int pageSize)
        {
            var response = await server.Value.GetAsync($"api/products?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<SimplifiedProduct>>(result);
        }

        public async Task<IDictionary<ServerInformation, Product>> GetProductByName(IDictionary<ServerInformation, HttpClient> servers, string name)
        {
            var products = new Dictionary<ServerInformation, Product>();

            foreach (var pair in servers)
            {
                var response = await pair.Value.GetAsync($"api/products/{name}");
                var result = await response.Content.ReadAsStringAsync();

                products.Add(pair.Key, JsonConvert.DeserializeObject<Product>(result));
            }

            return products;
        }

        public async void AddOrUpdateAsync(IDictionary<ServerInformation, HttpClient> servers, IDictionary<ServerInformation, Product> productsServersPair)
        {
            foreach (var pair in productsServersPair)
            {
                var client = servers[pair.Key];

                var serialized = JsonConvert.SerializeObject(pair.Value);
                var content = new StringContent(serialized, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/products", content);
                var result = await response.Content.ReadAsStringAsync();
            }
        }
    }
}