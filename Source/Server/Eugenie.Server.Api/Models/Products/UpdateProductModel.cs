namespace Eugenie.Server.Api.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    using Data.Models;

    public class UpdateProductModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        [MinLength(ValidationConstants.ProductNameMinLength)]
        public string Name { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public MeasureType Measure { get; set; }

        /// <summary>
        /// This quantity must be added to the existing quantity
        /// </summary>
        public decimal Quantity { get; set; }
    }
}