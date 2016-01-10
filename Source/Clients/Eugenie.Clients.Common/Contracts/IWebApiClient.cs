namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IWebApiClient
    {
        Task<int> GetProductsCountAsync(HttpClient client);

        Task<IEnumerable<Product>> GetProductsByPageAsync(HttpClient client, int page, int pageSize);

        Task<HttpStatusCode> AddOrUpdateAsync(HttpClient client, AddProductModel model);
    }
}