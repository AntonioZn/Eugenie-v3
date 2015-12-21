namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;

    using Data.Models;

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OldName { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public MeasureType Measure { get; set; }

        public decimal Quantity { get; set; }

        public virtual IEnumerable<Barcode> Barcodes { get; set; }

        public virtual IEnumerable<ExpirationDate> ExpirationDates { get; set; }
    }
}