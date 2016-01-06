namespace Eugenie.Clients.Common.Models
{
    public class Barcode
    {
        public Barcode(string barcode)
        {
            this.Value = barcode;
        }

        public string Value { get; set; }
    }
}