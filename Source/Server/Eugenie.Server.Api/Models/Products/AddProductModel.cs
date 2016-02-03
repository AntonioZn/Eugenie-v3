namespace Eugenie.Server.Api.Models.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    using Common.Constants;

    using Data.Models;

    public class AddProductModel
    {
        private string name;

        [Required]
        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = Regex.Replace(value.Trim(), @"\s+", " ");
            }
        }

        [MaxLength(ValidationConstants.ProductNameMaxLength)]
        public string OldName { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal? SellingPrice { get; set; }

        public MeasureType Measure { get; set; }

        public decimal? QuantityToAdd { get; set; }

        public ICollection<Barcode> Barcodes { get; set; }

        public ICollection<ExpirationDate> ExpirationDates { get; set; }
    }
}