namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IWebApiServerClient
    {
        Task<int> GetProductsCountAsync(KeyValuePair<ServerInformation, HttpClient> server);

        Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(KeyValuePair<ServerInformation, HttpClient> server, int page, int pageSize);

        Task<IDictionary<ServerInformation, Product>> GetProductByIdAsync(IDictionary<ServerInformation, HttpClient> servers, int id);

        void AddOrUpdateAsync(IDictionary<ServerInformation, HttpClient> servers, IDictionary<ServerInformation, Product> productsServersPair);
    }
}