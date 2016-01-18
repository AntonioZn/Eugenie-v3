namespace Eugenie.Server.Api.Models.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data.Models.DummyModels;

    public class SellProductsModel
    {
        [Required]
        public IEnumerable<IdQuantityPair> Products { get; set; }
    }
}