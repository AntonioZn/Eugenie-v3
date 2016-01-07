namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IWebApiClient
    {
        Task<int> GetProductsCountAsync(HttpClient client);

        Task<IEnumerable<Product>> GetProductsByPageAsync(HttpClient client, int page, int pageSize);

        void AddOrUpdateAsync(HttpClient client, Product product);
    }
}