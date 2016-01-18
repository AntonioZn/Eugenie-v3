namespace Eugenie.Data.Models
{
    using Newtonsoft.Json;

    public class ProductWithQuantity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}