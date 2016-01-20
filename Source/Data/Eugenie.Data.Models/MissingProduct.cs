namespace Eugenie.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common.Constants;

    using Newtonsoft.Json;

    public class MissingProduct
    {
        public MissingProduct()
        {
            this.Date = DateTime.Now;
        }

        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MissingProductMaxNameLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.BarcodeMaxLength)]
        [Index(IsUnique = true)]
        public string Barcode { get; set; }

        public DateTime Date { get; set; }
    }
}