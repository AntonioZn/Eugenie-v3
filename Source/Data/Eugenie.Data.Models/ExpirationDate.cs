namespace Eugenie.Data.Models
{
    using System;

    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ExpirationDate
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        [JsonIgnore]
        public int ProductId { get; set; }
        
        [MaxLength(100)]
        public string Batch { get; set; }

        public DateTime Date { get; set; }

        public decimal Quantity { get; set; }
    }
}