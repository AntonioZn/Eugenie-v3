namespace Eugenie.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Shipment
    {
        private ICollection<ProductWithQuantity> products;

        public Shipment()
        {
            this.products = new HashSet<ProductWithQuantity>();
            this.Date = DateTime.Now;
        }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        public virtual Report Report { get; set; }

        public decimal Total { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<ProductWithQuantity> Products
        {
            get
            {
                return this.products;
            }
            set
            {
                this.products = value;
            }
        }

        public void AddProduct(ProductWithQuantity productWithQuantity)
        {
            this.Products.Add(productWithQuantity);
            this.Total += productWithQuantity.Product.SellingPrice * productWithQuantity.Quantity;
        }
    }
}