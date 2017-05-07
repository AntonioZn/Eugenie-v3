namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    using Helpers;

    public class ExpiringProductsViewModel : ViewModelBase
    {
        private readonly ServerManager manager;
        private readonly ObservableCollection<Product> products = new ObservableCollection<Product>();
        private DateTime date = DateTime.Now;

        public ExpiringProductsViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedStoreChanged += this.OnSelectedStoreChanged;
        }

        public IEnumerable<Product> Products
        {
            get => this.products;
            set
            {
                this.products.Clear();
                value.ForEach(this.products.Add);
            }
        }

        public DateTime Date
        {
            get => this.date;
            set
            {
                this.Set(() => this.Date, ref this.date, value);
                this.Products = this.manager.SelectedStore?.Products.Where(x => x.ExpirationDates.Any(y => y.Date <= this.Date)) ?? Enumerable.Empty<Product>();
            }
        }

        private void OnSelectedStoreChanged(object sender, EventArgs e)
        {
            this.Products = this.manager.SelectedStore?.Products.Where(x => x.ExpirationDates.Any(y => y.Date <= this.Date)) ?? Enumerable.Empty<Product>();
        }
    }
}