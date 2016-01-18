namespace Eugenie.Clients.Common.WebApiModels
{
    using System;

    public class MissingProduct
    {
        public string Name { get; set; }

        public string Barcode { get; set; }

        public DateTime Date { get; set; }

        public override int GetHashCode()
        {
            return this.Barcode.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var missingProduct = obj as MissingProduct;
            return missingProduct != null && this.Barcode.Equals(((MissingProduct) obj).Barcode);
        }
    }
}