namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;

    using Models;

    public interface IWebApiServerClient
    {
        Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize);

        Task<int> GetProductsCountAsync();

        Task<IDictionary<ServerInformation, Product>> GetProductByIdAsync(int id);

        void UpdateAsync(IDictionary<ServerInformation, Product> productsServersPair);
    }
}