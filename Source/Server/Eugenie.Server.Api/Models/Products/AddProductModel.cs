namespace Eugenie.Server.Api.Models.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    using Data.Models;

    public class AddProductModel
    {
        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string Name { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public MeasureType Measure { get; set; }

        public decimal Quantity { get; set; }

        public string Barcode { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}