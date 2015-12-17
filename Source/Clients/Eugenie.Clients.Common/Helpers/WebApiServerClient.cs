namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using Newtonsoft.Json;

    public class WebApiServerClient : IWebApiServerClient
    {
        private readonly IServersManager serversManager;

        public WebApiServerClient(IServersManager serversManager)
        {
            this.serversManager = serversManager;
        }

        //TODO: get products from fastest server
        public async Task<int> GetProductsCountAsync()
        {
            var response = await this.serversManager.FastestServer.GetAsync("api/products");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(result);
        }

        //TODO: get products from fastest server
        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize)
        {
            var response = await this.serversManager.FastestServer.GetAsync($"api/products?page={page}&pageSize={pageSize}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<SimplifiedProduct>>(result);
        }
    }
}