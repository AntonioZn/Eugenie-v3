namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Contracts;

    using GalaSoft.MvvmLight;

    using Models;

    public class ProductsCache : ViewModelBase, IProductsCache
    {
        private ObservableCollection<Product> products;

        public ProductsCache()
        {
            this.ProductsPerServer = new ConcurrentDictionary<ServerInformation, ObservableCollection<Product>>();
        }
        
        public IEnumerable<Product> Products
        {
            get
            {
                return this.products ?? (this.products = new ObservableCollection<Product>());
            }

            set
            {
                this.products = this.products ?? new ObservableCollection<Product>();

                this.products.Clear();
                foreach (var product in value)
                {
                    this.products.Add(product);
                }
            }
        }
        public IDictionary<ServerInformation, ObservableCollection<Product>> ProductsPerServer { get; set; }
    }
}