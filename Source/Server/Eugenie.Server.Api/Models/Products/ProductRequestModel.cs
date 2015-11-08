

namespace Eugenie.Server.Api.Models.Products
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    using Data.Models;

    //Add(string name, decimal buyingPrice = 0, decimal sellingPrice = 0, MeasureType measure = MeasureType.бр, decimal quantity = 0, string barcode = null, DateTime? expirationDate = null);
    public class ProductRequestModel
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