namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface IWebApiServerClient
    {
        Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize);

        Task<int> GetProductsCountAsync();
    }
}