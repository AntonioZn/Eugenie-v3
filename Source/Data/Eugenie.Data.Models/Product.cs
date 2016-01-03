namespace Eugenie.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Constants;

    public class Product
    {
        private ICollection<Barcode> barcodes;
        private ICollection<ExpirationDate> expirationDates;

        public Product()
        {
            this.barcodes = new HashSet<Barcode>();
            this.expirationDates = new HashSet<ExpirationDate>();
            this.Measure = MeasureType.бр;
        }

        public int Id { get; set; }

        [Required]
        [Index("Name", IsUnique = true)]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        [MinLength(ValidationConstants.ProductNameMinLength)]
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

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}