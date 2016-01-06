namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;

    using Models;

    public interface IProductsCache
    {
        IEnumerable<SimplifiedProduct> SimplifiedProducts { get; set; }
    }
}