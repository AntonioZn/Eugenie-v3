namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Newtonsoft.Json;

    public class Product
    {
        private ObservableCollection<ExpirationDate> expirationDates;
         
        public string Name { get; set; }
        
        public string NewName { get; set; }

        public decimal? BuyingPrice { get; set; }

        public decimal? SellingPrice { get; set; }

        public MeasureType? Measure { get; set; }
        
        public decimal? Quantity { get; set; }
        
        [JsonIgnore]
        public decimal? QuantityToAdd { get; set; }

        public IEnumerable<Barcode> Barcodes { get; set; }

        public ICollection<ExpirationDate> ExpirationDates
        {
            get
            {
                if (this.expirationDates == null)
                {
                    this.expirationDates = new ObservableCollection<ExpirationDate>();
                }

                return this.expirationDates;
            }

            set
            {
                if (this.expirationDates == null)
                {
                    this.expirationDates = new ObservableCollection<ExpirationDate>();
                }

                this.expirationDates.Clear();
                foreach (var expirationDate in value)
                {
                    this.expirationDates.Add(expirationDate);
                }
            }
        }
    }
}