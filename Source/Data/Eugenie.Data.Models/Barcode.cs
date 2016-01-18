namespace Eugenie.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Constants;

    using Newtonsoft.Json;

    public class Barcode
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        [Required]
        [Index("Value", IsUnique = true)]
        [MaxLength(ValidationConstants.BarcodeMaxLength)]
        public string Value { get; set; }
    }
}