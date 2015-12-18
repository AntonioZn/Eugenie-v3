namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using Common.Models;

    using Data.Models;

    public class ProductInformationViewModel
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, Product> products)
        {
            this.Products = products;
            this.FirstProduct = this.Products.First().Value;
        }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public Product FirstProduct { get; set; }
    }
}