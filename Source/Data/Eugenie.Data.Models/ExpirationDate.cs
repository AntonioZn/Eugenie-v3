namespace Eugenie.Data.Models
{
    using System;

    public class ExpirationDate
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public DateTime Date { get; set; }
    }
}