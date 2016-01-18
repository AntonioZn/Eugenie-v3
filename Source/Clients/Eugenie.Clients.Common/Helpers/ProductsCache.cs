namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Contracts;

    using GalaSoft.MvvmLight;

    using Models;

    using WebApiModels;

    public class ProductsCache : ViewModelBase, IProductsCache
    {
        private ObservableCollection<Product> products;
        private ObservableCollection<MissingProduct> missingProducts;

        public ProductsCache()
        {
            this.ProductsPerServer = new ConcurrentDictionary<ServerInformation, ICollection<Product>>();
            this.ReportsPerServer = new ConcurrentDictionary<ServerInformation, IEnumerable<Report>>();
        }

        public IEnumerable<Product> MainProducts
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

        public IDictionary<ServerInformation, ICollection<Product>> ProductsPerServer { get; }

        public IDictionary<ServerInformation, IEnumerable<Report>> ReportsPerServer { get; }

        public ICollection<MissingProduct> MissingProducts
        {
            get
            {
                return this.missingProducts ?? (this.missingProducts = new ObservableCollection<MissingProduct>());
            }

            set
            {
                this.missingProducts = this.missingProducts ?? new ObservableCollection<MissingProduct>();

                this.missingProducts.Clear();
                foreach (var missingProduct in value)
                {
                    this.missingProducts.Add(missingProduct);
                }
            }
        }

        public void SetMainProducts()
        {
            this.MainProducts = this.ProductsPerServer.FirstOrDefault(x => x.Value.Any()).Value;
        }
    }
}