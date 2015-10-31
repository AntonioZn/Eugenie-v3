using System.Collections;
using System.Collections.Generic;

namespace Eugenie.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        private ICollection<Barcode> barcodes;
        private ICollection<ExpirationDate> expirationDates;

        public Product()
        {
            this.barcodes = new HashSet<Barcode>();
            this.expirationDates = new HashSet<ExpirationDate>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
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