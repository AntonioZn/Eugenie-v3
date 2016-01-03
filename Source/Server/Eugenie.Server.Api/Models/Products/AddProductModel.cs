namespace Eugenie.Server.Api.Models.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    using Data.Models;

    public class AddProductModel
    {
        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string NewName { get; set; }

        public decimal? BuyingPrice { get; set; }

        public decimal? SellingPrice { get; set; }
        
        public MeasureType? Measure { get; set; }

        public decimal? Quantity { get; set; }

        public IEnumerable<string> Barcodes { get; set; }

        public IEnumerable<DateTime> ExpirationDates { get; set; }
    }
}