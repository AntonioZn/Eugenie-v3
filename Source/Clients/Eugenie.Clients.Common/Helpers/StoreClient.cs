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

    using Exceptions;

    using Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Sv.Wpf.Core.Helpers;

    using WebApiModels;

    public class StoreClient : IDisposable
    {
        private readonly ResilientHttpClient client;

        public StoreClient(string address)
        {
            this.client = new ResilientHttpClient();
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.BaseAddress = new Uri(address);

            this.client.Timeout = TimeSpan.FromSeconds(5);
            this.client.RetryAfterTimeout = false;
            this.client.RetryCount = 0;
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        public async Task AuthenticateAsync(string username, string password, CancellationToken cToken = default(CancellationToken))
        {
            var content = new StringContent($"grant_type=password&username={username}&password={password}");
            var response = await this.client.PostAsync("api/account/token", content, cToken);

            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync();
                throw new LoginException(text);
            }

            var result = await response.Content.ReadAsStringAsync();
            var token = JObject.Parse(result)["access_token"].ToString();
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ICollection<Report>> GetReportsAsync(CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync("api/reports", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Report>>(result);
        }

        public async Task<ReportDetailsResponse> GetReportDetailsAsync(DateTime date, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/reports?date={date.Year}-{date.Month}-{date.Day}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReportDetailsResponse>(result);
        }

        public async Task<ICollection<Seller>> GetSellersAsync(CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync("api/sellers", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Seller>>(result);
        }

        public async Task<ReportDetailsResponse> GetDealsForSellerAsync(string username, DateTime start, DateTime end, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/deals?username={username}&start={start.Year}-{start.Month}-{start.Day}&end={end.Year}-{end.Month}-{end.Day}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReportDetailsResponse>(result);
        }

        public async Task<ICollection<Product>> GetProductsAsync(CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync("api/products", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Product>>(result);
        }

        public async Task<HttpStatusCode> AddOrUpdateAsync(AddProductModel model, CancellationToken cToken = default(CancellationToken))
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync("api/products", content, cToken);

            return response.StatusCode;
        }

        public async Task<ICollection<MissingProduct>> GetMissingProductsAsync(CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/missingProducts", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<MissingProduct>>(result);
        }

        public async Task<UserInfoResponse> GetUserInfoAsync(CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/account/userinfo", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserInfoResponse>(result);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/products?name={name}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }

        public async Task<Product> GetProductByIdAsync(int id, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/products?id={id}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(result);
        }

        public async Task<Product> GetProductByBarcodeAsync(string barcode, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/products?barcode={barcode}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Product>(result);
        }

        public async Task<HttpStatusCode> WasteProductsAsync(IEnumerable<IdQuantityPair> model, CancellationToken cToken = default(CancellationToken))
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync("api/deals/waste", content, cToken);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> SellProductsAsync(IEnumerable<IdQuantityPair> model, CancellationToken cToken = default(CancellationToken))
        {
            var serialized = JsonConvert.SerializeObject(model);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync("api/deals/sell", content, cToken);

            return response.StatusCode;
        }

        public async Task<IEnumerable<Product>> GetExpiringProductsAsync(int days, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/products?days={days}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }

        public async Task<IEnumerable<Product>> GetLowQuantityProductsAsync(decimal quantity, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.GetAsync($"api/products?quantity={quantity}", cToken);

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
        }

        public async Task<HttpStatusCode> DeleteProductAsync(string name, CancellationToken cToken = default(CancellationToken))
        {
            var response = await this.client.DeleteAsync($"api/products?name={name}", cToken);
            return response.StatusCode;
        }
    }
}
