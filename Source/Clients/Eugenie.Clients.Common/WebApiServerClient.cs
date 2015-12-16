namespace Eugenie.Clients.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using System.Net.Http;
    using System.Net.Http.Headers;

    using Models;

    using Newtonsoft.Json;

    public class WebApiServerClient
    {
        private readonly HttpClient client;

        public WebApiServerClient(Uri address, string authToken)
        {
            this.client = new HttpClient { BaseAddress = address };
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<int> GetProductsCountAsync()
        {
            var response = await this.client.GetAsync("api/products");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(result);
            }

            throw new ArgumentException();
        }

        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize)
        {
            var response = await this.client.GetAsync($"api/products?page={page}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<SimplifiedProduct>>(result);
            }

            throw new ArgumentException();
        }

        //public async Task<>
    }
}
