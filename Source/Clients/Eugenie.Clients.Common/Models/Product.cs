namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;

    using Data.Models;

    using Newtonsoft.Json;

    public class Product
    {
        public string Name { get; set; }
        
        public string NewName { get; set; }

        public decimal? BuyingPrice { get; set; }

        public decimal? SellingPrice { get; set; }

        public MeasureType? Measure { get; set; }
        
        public decimal? Quantity { get; set; }
        
        [JsonIgnore]
        public decimal? QuantityToAdd { get; set; }

        public IEnumerable<Barcode> Barcodes { get; set; }

        public ICollection<ExpirationDate> ExpirationDates { get; set; }
    }
}