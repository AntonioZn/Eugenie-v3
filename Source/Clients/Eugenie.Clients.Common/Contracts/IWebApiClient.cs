namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IWebApiClient
    {
        Task<IEnumerable<Product>> GetProductsAsync(HttpClient client);

        Task<HttpStatusCode> AddOrUpdateAsync(HttpClient client, AddProductModel model);

        Task<ISet<MissingProduct>> GetMissingProductsAsync(HttpClient client);
    }
}