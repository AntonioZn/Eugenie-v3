namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using Newtonsoft.Json;

    using WebApiModels;

    public class WebApiClient : IWebApiClient
    {
        public async Task<ICollection<Report>> GetReportsAsync(HttpClient client)
        {
            var response = await client.GetAsync("api/reports");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Report>>(result);
        }

        public async Task<ReportDetails> GetReportDetailsAsync(HttpClient client, DateTime date)
        {
            var response = await client.GetAsync($"api/reports/{date}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReportDetails>(result);
        }

        public async Task<ICollection<Product>> GetProductsAsync(HttpClient client)
        {
            var response = await client.GetAsync("api/products");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Product>>(result);
        }

        public async Task<HttpStatusCode> AddOrUpdateAsync(HttpClient client, AddProductModel model)
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/products", content);

            return response.StatusCode;
        }

        public async Task<IEnumerable<MissingProduct>> GetMissingProductsAsync(HttpClient client)
        {
            var response = await client.GetAsync($"api/missingProducts");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MissingProduct>>(result);
        }
    }
}