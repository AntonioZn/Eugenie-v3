namespace Eugenie.Data.Models
{
    public class ProductWithQuantity
    {
        public int Id { get; set; }

        public decimal Quantity { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}