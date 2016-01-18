﻿namespace Eugenie.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Sell
    {
        private ICollection<ProductWithQuantity> products;

        public Sell()
        {
            this.products = new HashSet<ProductWithQuantity>();
            this.Date = DateTime.UtcNow;
        }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        public virtual User Seller { get; set; }

        [JsonIgnore]
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