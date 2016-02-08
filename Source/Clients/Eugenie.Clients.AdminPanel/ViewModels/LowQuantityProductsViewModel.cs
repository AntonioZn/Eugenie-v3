namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;
    using Common.Models;
    using Common.Еxtensions;

    using Contracts;

    using GalaSoft.MvvmLight;

    public class LowQuantityProductsViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private ObservableCollection<Product> products;
        private string quantity;

        public LowQuantityProductsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedServerChanged += this.OnSelectedServerChanged;
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
                value.ForEach(this.products.Add);
            }
        }

        public string Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                this.Set(() => this.Quantity, ref this.quantity, value);
                this.GetProducts();
            }
        }

        private void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.GetProducts();
        }

        private void GetProducts()
        {
            if (this.manager.SelectedServer == null)
            {
                this.products.Clear();
                return;
            }

            decimal parsedQuantity;
            if (decimal.TryParse(this.Quantity, out parsedQuantity))
            {
                this.Products = this.manager.Cache.ProductsPerServer[this.manager.SelectedServer].Where(x => x.Quantity <= parsedQuantity).OrderByDescending(x => x.Quantity);
            }
        }
    }
}