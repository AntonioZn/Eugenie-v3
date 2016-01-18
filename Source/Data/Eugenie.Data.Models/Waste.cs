﻿namespace Eugenie.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Waste
    {
        private ICollection<ProductWithQuantity> products;

        public Waste()
        {
            this.products = new HashSet<ProductWithQuantity>();
            this.Date = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        public virtual User Seller { get; set; }

        [Required]
        public virtual DailyEarning DailyEarning { get; set; }

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