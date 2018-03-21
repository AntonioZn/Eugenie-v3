namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Models;

    using Sv.Wpf.Core.Extensions;
    using Sv.Wpf.Core.Mvvm;

    public class BasketViewModel : ViewModelBase
    {
        private ObservableCollection<Product> products;
        private decimal totalPrice;

        public ICollection<Product> Products
        {
            get => this.products ?? (this.products = new ObservableCollection<Product>());

            set
            {
                this.products = this.products ?? new ObservableCollection<Product>();
                this.products.Clear();
                value.ForEach(this.products.Add);
            }
        }

        public decimal TotalPrice
        {
            get => this.totalPrice;

            set => this.Set(ref this.totalPrice, value);
        }

        public void Add(Product product)
        {
            var existingProduct = this.Products.FirstOrDefault(x => x.Id == product.Id);
            if (existingProduct != null)
            {
                this.Products.Remove(existingProduct);
            }

            this.RemoveCoupons(product);

            this.Products.Add(product);
            this.CalculatePrice();
        }

        public void Delete(Product product)
        {
            this.RemoveCoupons(product);

            this.Products.Remove(product);
            this.CalculatePrice();
        }

        public void Clear()
        {
            this.Products.Clear();
            this.TotalPrice = 0;
        }

        private void RemoveCoupons(Product product)
        {
            if (product.Name.Contains("хляб") && !product.Name.Contains("$"))
            {
                var coupons = this.Products.Where(x => x.Name.Contains("$")).ToList();
                foreach (var coupon in coupons)
                {
                    this.Products.Remove(coupon);
                }
            }
        }

        private void CalculatePrice()
        {
            this.TotalPrice = this.Products.Sum(x => x.SellingPrice * x.Quantity).GetValueOrDefault();
        }
    }
}