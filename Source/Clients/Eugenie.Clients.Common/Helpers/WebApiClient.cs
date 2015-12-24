namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using Newtonsoft.Json;

    public class WebApiClient : IWebApiClient
    {
        public async Task<int> GetProductsCountAsync(HttpClient client)
        {
            var response = await client.GetAsync("api/products");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(result);
        }

        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(HttpClient client, int page, int pageSize)
        {
            var response = await client.GetAsync($"api/products?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<SimplifiedProduct>>(result);
        }

        public async Task<Product> GetProductByName(HttpClient client, string name)
        {
            var response = await client.GetAsync($"api/products/{name}");
            var result = await response.Content.ReadAsStringAsync();

            var product = JsonConvert.DeserializeObject<Product>(result);

            return product;
        }

        public async void AddOrUpdateAsync(HttpClient client, Product product)
        {
            var serialized = JsonConvert.SerializeObject(product);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/products", content);
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}