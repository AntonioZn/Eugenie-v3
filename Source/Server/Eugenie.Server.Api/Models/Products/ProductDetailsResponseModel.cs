namespace Eugenie.Server.Api.Models.Products
{
    using System.Collections.Generic;

    using Data.Models;

    public class ProductDetailsResponseModel
    {
        private ICollection<Barcode> barcodes;
        private ICollection<ExpirationDate> expirationDates;

        public ProductDetailsResponseModel()
        {
            this.barcodes = new HashSet<Barcode>();
            this.expirationDates = new HashSet<ExpirationDate>();
        }

        public int Id { get; set; }
        
        public string Name { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public MeasureType Measure { get; set; }

        public decimal Quantity { get; set; }

        public virtual ICollection<Barcode> Barcodes
        {
            get
            {
                return this.barcodes;
            }

            set
            {
                this.barcodes = value;
            }
        }

        public virtual ICollection<ExpirationDate> ExpirationDates
        {
            get
            {
                return this.expirationDates;
            }

            set
            {
                this.expirationDates = value;
            }
        }
    }
}