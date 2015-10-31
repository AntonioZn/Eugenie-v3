namespace Eugenie.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MissingProduct
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Barcode { get; set; }

        public DateTime Date { get; set; }
    }
}