namespace Eugenie.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Barcode
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }

        [Required]
        [MaxLength(20)]
        public string Value { get; set; }
    }
}