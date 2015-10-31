using System.Collections.Generic;

namespace Eugenie.Data.Models
{
    using System;

    public class Waste
    {
        private ICollection<ProductWithQuantity> products;

        public Waste()
        {
            this.products = new HashSet<ProductWithQuantity>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User Seller { get; set; }

        public decimal Total { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<ProductWithQuantity> Products
        {
            get { return this.products; }
            set { this.products = value; }
        }
    }
}