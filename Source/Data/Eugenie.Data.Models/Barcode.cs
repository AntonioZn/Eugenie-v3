namespace Eugenie.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Barcode
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        [MaxLength(20)]
        public string Value { get; set; }
    }
}