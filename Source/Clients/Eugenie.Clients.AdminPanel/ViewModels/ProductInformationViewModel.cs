namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

    using Common.Models;

    using Data.Models;

    public class ProductInformationViewModel
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, Product> products, SimplifiedProduct simpleProduct)
        {
            this.Products = products;
            this.FirstProduct = simpleProduct;
            this.Measures = new List<MeasureType> { MeasureType.бр, MeasureType.кг, MeasureType.л };
        }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public SimplifiedProduct FirstProduct { get; set; }

        public ICollection<MeasureType> Measures { get; set; }
    }
}