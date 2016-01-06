namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;

    using Contracts;

    using Models;

    public class ProductsCache : IProductsCache
    {
        public ProductsCache()
        {
            
        }

        public IEnumerable<SimplifiedProduct> SimplifiedProducts { get; set; }
    }
}