namespace Eugenie.Clients.Admin.Models
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Common.Models;
    using Common.WebApiModels;

    public class Cache
    {
        public Cache()
        {
            this.ProductsPerServer = new ConcurrentDictionary<ServerInformation, ICollection<Product>>();
            this.ReportsPerServer = new ConcurrentDictionary<ServerInformation, IEnumerable<Report>>();
            this.MissingProductsPerServer = new ConcurrentDictionary<ServerInformation, IEnumerable<MissingProduct>>();
            this.SellersPerServer = new ConcurrentDictionary<ServerInformation, IEnumerable<Seller>>();
        }

        public IDictionary<ServerInformation, ICollection<Product>> ProductsPerServer { get; }

        public IDictionary<ServerInformation, IEnumerable<Report>> ReportsPerServer { get; }

        public IDictionary<ServerInformation, IEnumerable<MissingProduct>> MissingProductsPerServer { get; }

        public IDictionary<ServerInformation, IEnumerable<Seller>> SellersPerServer { get; }
    }
}