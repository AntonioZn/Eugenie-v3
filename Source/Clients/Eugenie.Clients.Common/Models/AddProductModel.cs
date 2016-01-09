namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;

    public class AddProductModel
    {
        public string Name { get; set; }
        
        public string OldName { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal? SellingPrice { get; set; }
        
        public MeasureType Measure { get; set; }

        public decimal? QuantityToAdd { get; set; }

        public ICollection<Barcode> Barcodes { get; set; }

        public ICollection<ExpirationDate> ExpirationDates { get; set; }
    }
}