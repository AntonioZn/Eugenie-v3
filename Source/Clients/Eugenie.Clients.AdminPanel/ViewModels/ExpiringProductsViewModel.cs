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

    public class ExpiringProductsViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private ObservableCollection<Product> products;
        private DateTime date;

        public ExpiringProductsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedServerChanged += this.OnSelectedServerChanged;
            this.Date = DateTime.Today;
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

        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.Set(() => this.Date, ref this.date, value);
                if (this.manager.SelectedServer != null)
                {
                    this.Products = this.manager.Cache.ProductsPerServer[this.manager.SelectedServer].Where(x => x.ExpirationDates.Any(y => y.Date <= this.Date));
                }
            }
        }

        private void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.Products = this.manager.SelectedServer == null ? Enumerable.Empty<Product>() :
                this.manager.Cache.ProductsPerServer[this.manager.SelectedServer].Where(x => x.ExpirationDates.Any(y => y.Date <= this.Date));
        }
    }
}