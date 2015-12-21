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

        [JsonIgnore]
        public virtual Product Product { get; set; }

        //TODO: review ?
        //[Required]
        [MaxLength(20)]
        public string Batch { get; set; }

        public DateTime Date { get; set; }
    }
}