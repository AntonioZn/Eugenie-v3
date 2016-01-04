namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;

    using Models;

    public class ProductsCache : IProductsCache
    {
        public ProductsCache()
        {
            
        }

        public IEnumerable<SimplifiedProduct> SimplifiedProducts { get; set; }
    }
}