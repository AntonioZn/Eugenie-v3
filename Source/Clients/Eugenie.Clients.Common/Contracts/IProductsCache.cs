namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Models;

    public interface IProductsCache
    {
        IEnumerable<Product> Products { get; set; }

        IDictionary<ServerInformation, ObservableCollection<Product>> ProductsPerServer { get; set; }
    }
}