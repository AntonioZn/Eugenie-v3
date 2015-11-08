namespace Eugenie.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class MissingProduct
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MissingProductMaxNameLength)]
        public string Name { get; set; }

        [Required]
        public string Barcode { get; set; }

        public DateTime Date { get; set; }
    }
}