namespace Eugenie.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class MissingProduct
    {
        public MissingProduct()
        {
            this.Date = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MissingProductMaxNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.BarcodeMaxLength)]
        public string Barcode { get; set; }

        public DateTime Date { get; set; }
    }
}