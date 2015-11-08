namespace Eugenie.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

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
        [MaxLength(ValidationConstants.BarcodeMaxLength)]
        public string Value { get; set; }
    }
}