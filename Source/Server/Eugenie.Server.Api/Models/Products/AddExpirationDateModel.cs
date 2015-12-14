namespace Eugenie.Server.Api.Models.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddExpirationDateModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}