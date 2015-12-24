namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IWebApiClient
    {
        Task<int> GetProductsCountAsync(HttpClient client);

        Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(HttpClient client, int page, int pageSize);

        Task<Product> GetProductByName(HttpClient client, string name);

        void AddOrUpdateAsync(HttpClient client, Product product);
    }
}