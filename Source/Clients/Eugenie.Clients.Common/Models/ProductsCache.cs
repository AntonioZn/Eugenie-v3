namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using WebApiModels;

    public class ProductsCache
    {
        public ProductsCache()
        {
            this.ProductsPerServer = new ConcurrentDictionary<ServerInformation, ICollection<Product>>();
            this.ReportsPerServer = new ConcurrentDictionary<ServerInformation, IEnumerable<Report>>();
            this.MissingProductsPerServer = new ConcurrentDictionary<ServerInformation, IEnumerable<MissingProduct>>();
        }

        public IDictionary<ServerInformation, ICollection<Product>> ProductsPerServer { get; }

        public IDictionary<ServerInformation, IEnumerable<Report>> ReportsPerServer { get; }

        public IDictionary<ServerInformation, IEnumerable<MissingProduct>> MissingProductsPerServer { get; }
    }
}