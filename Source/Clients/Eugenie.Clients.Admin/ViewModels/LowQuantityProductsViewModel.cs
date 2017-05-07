namespace Eugenie.Clients.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    using Helpers;

    public class LowQuantityProductsViewModel : ViewModelBase
    {
        private readonly ServerManager manager;
        private ObservableCollection<Product> products;
        private string quantity;

        public LowQuantityProductsViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedStoreChanged += (s, e) => this.GetProducts();
        }

        public IEnumerable<Product> Products
        {
            get => this.products ?? (this.products = new ObservableCollection<Product>());
            set
            {
                this.products = this.products ?? new ObservableCollection<Product>();
                this.products.Clear();
                value.ForEach(this.products.Add);
            }
        }

        public string Quantity
        {
            get => this.quantity;
            set
            {
                this.Set(() => this.Quantity, ref this.quantity, value);
                this.GetProducts();
            }
        }

        private void GetProducts()
        {
            if (this.manager.SelectedStore == null)
            {
                this.products.Clear();
                return;
            }
            
            if (decimal.TryParse(this.Quantity, out var parsedQuantity))
            {
                this.Products = this.manager.SelectedStore.Products.Where(x => x.Quantity <= parsedQuantity).OrderByDescending(x => x.Quantity);
            }
        }
    }
}