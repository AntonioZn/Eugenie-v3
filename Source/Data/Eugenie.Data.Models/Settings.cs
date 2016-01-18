namespace Eugenie.Data.Models
{
    using System;

    using Newtonsoft.Json;

    public class Settings
    {
        [JsonIgnore]
        public int Id { get; set; }
        public DateTime OpenTime { get; set; }

        public DateTime CloseTime { get; set; }
    }
}