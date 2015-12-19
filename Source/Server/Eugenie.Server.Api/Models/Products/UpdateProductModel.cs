namespace Eugenie.Server.Api.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    using Data.Models;

    public class UpdateProductModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        [MinLength(ValidationConstants.ProductNameMinLength)]
        public string Name { get; set; }

        [Required]
        public decimal BuyingPrice { get; set; }

        [Required]
        public decimal SellingPrice { get; set; }

        [Required]
        public MeasureType Measure { get; set; }

        [Required]
        public decimal Quantity { get; set; }
    }
}