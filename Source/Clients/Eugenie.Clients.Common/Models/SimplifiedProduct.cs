namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
    using Data.Models;

    public class SimplifiedProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MeasureType Measure { get; set; }

        public IEnumerable<Barcode> Barcodes { get; set; }
    }
}