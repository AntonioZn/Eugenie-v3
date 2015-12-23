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

        Task<IDictionary<ServerInformation, Product>> GetProductByName(IDictionary<ServerInformation, HttpClient> servers, string name);

        void AddOrUpdateAsync(IDictionary<ServerInformation, HttpClient> servers, IDictionary<ServerInformation, Product> productsServersPair);
    }
}