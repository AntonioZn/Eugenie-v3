namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Net;
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

        public async Task<IEnumerable<Product>> GetProductsByPageAsync(HttpClient client, int page, int pageSize)
        {
            var response = await client.GetAsync($"api/products?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HashSet<Product>>(result);
        }

        public async Task<HttpStatusCode> AddOrUpdateAsync(HttpClient client, AddProductModel model)
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/products", content);

            return response.StatusCode;
        }
    }
}