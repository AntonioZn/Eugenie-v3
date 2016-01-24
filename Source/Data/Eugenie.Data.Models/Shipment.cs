namespace Eugenie.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class Shipment
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        public virtual Report Report { get; set; }

        public virtual Product Product { get; set; }

        public decimal Quantity { get; set; }
    }
}