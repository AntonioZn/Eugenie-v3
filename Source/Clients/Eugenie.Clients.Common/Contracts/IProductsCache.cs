namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;

    using Models;

    using WebApiModels;

    public interface IProductsCache
    {
        IEnumerable<Product> MainProducts { get; set; }

        IDictionary<ServerInformation, ICollection<Product>> ProductsPerServer { get; }

        IDictionary<ServerInformation, IEnumerable<Report>> ReportsPerServer { get; }

        ICollection<MissingProduct> MissingProducts { get; set; }

        void SetMainProducts();
    }
}