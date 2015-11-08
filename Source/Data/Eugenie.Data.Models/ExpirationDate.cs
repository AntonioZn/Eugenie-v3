namespace Eugenie.Data.Models
{
    using System;

    using Newtonsoft.Json;

    public class ExpirationDate
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }

        public DateTime Date { get; set; }
    }
}