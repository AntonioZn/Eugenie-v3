namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using WebApiModels;

    public class StoreClient : IDisposable
    {
        private readonly HttpClient client;

        public StoreClient(string address)
        {
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.BaseAddress = new Uri(address);

            this.client.Timeout = TimeSpan.FromSeconds(30);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            try
            {
                var content = new StringContent($"grant_type=password&username={username}&password={password}");
                var response = await this.client.PostAsync("api/account/token", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var token = JObject.Parse(result)["access_token"].ToString();
                    this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public async Task<ICollection<Report>> GetReportsAsync()
        {
            var response = await this.client.GetAsync("api/reports");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Report>>(result);
        }

        public async Task<ReportDetailsResponse> GetReportDetailsAsync(DateTime date)
        {
            var response = await this.client.GetAsync($"api/reports?date={date.Year}-{date.Month}-{date.Day}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReportDetailsResponse>(result);
        }

        public async Task<ICollection<Seller>> GetSellersAsync()
        {
            var response = await this.client.GetAsync("api/sellers");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Seller>>(result);
        }

        public async Task<ReportDetailsResponse> GetDealsForSeller(string username, DateTime start, DateTime end)
        {
            var response = await this.client.GetAsync($"api/deals?username={username}&start={start.Year}-{start.Month}-{start.Day}&end={end.Year}-{end.Month}-{end.Day}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReportDetailsResponse>(result);
        }

        public async Task<ICollection<Product>> GetProductsAsync()
        {
            var response = await this.client.GetAsync("api/products");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Product>>(result);
        }

        public async Task<HttpStatusCode> AddOrUpdateAsync(AddProductModel model)
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync("api/products", content);

            return response.StatusCode;
        }

        public async Task<ICollection<MissingProduct>> GetMissingProductsAsync()
        {
            var response = await this.client.GetAsync($"api/missingProducts");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<MissingProduct>>(result);
        }

        public async Task<UserInfoResponse> GetUserInfo()
        {
            var response = await this.client.GetAsync($"api/account/userinfo");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserInfoResponse>(result);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken token)
        {
            var response = await this.client.GetAsync($"api/products?name={name}", token);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }

        public async Task<Product> GetProductById(int id)
        {
            var response = await this.client.GetAsync($"api/products?id={id}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(result);
        }

        public async Task<Product> GetProductByBarcode(string barcode)
        {
            var response = await this.client.GetAsync($"api/products?barcode={barcode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(result);
        }

        public async Task<HttpStatusCode> WasteProductsAsync(IEnumerable<IdQuantityPair> model)
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync("api/deals/waste", content);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> SellProductsAsync(IEnumerable<IdQuantityPair> model)
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync("api/deals/sell", content);

            return response.StatusCode;
        }

        public async Task<IEnumerable<Product>> GetExpiringProductsAsync(int days)
        {
            var response = await this.client.GetAsync($"api/products?days={days}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }

        public async Task<IEnumerable<Product>> GetLowQuantityProducts(decimal quantity)
        {
            var response = await this.client.GetAsync($"api/products?quantity={quantity}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }

        public async Task<HttpStatusCode> DeleteProductAsync(string name)
        {
            var response = await this.client.DeleteAsync($"api/products?name={name}");
            return response.StatusCode;
        }
    }
}
