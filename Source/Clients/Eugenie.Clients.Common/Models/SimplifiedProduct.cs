namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;

    using Data.Models;

    public class SimplifiedProduct
    {
        public SimplifiedProduct()
        {
            
        }

        public SimplifiedProduct(int id, string name, decimal buyingPrice, MeasureType measure, IEnumerable<Barcode> barcodes)
        {
            this.Id = id;
            this.Name = name;
            this.BuyingPrice = buyingPrice;
            this.Measure = measure;
            this.Barcodes = barcodes;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal BuyingPrice { get; set; }

        public MeasureType Measure { get; set; }

        public IEnumerable<Barcode> Barcodes { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}