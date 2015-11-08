namespace Eugenie.Server.Api.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class AddBarcodeModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(ValidationConstants.BarcodeMaxLength)]
        public string Barcode { get; set; }
    }
}