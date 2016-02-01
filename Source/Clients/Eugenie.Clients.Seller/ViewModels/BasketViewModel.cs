namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    public class BasketViewModel : ViewModelBase
    {
        private ObservableCollection<Product> products;
        private decimal totalPrice;

        public ICollection<Product> Products
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

        public decimal TotalPrice
        {
            get
            {
                return this.totalPrice;
            }

            set
            {
                this.Set(() => this.TotalPrice, ref this.totalPrice, value);
            }
        }

        public void Add(Product product)
        {
            this.Products.Add(product);
            this.CalculatePrice();
        }

        public void Delete(Product product)
        {
            this.Products.Remove(product);
            this.CalculatePrice();;
        }

        public void CalculatePrice()
        {
            this.TotalPrice = this.Products.Sum(x => x.SellingPrice * x.Quantity).GetValueOrDefault();
        }
    }
}
