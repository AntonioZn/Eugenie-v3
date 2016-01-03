namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Common.Models;

    using Data.Models;

    using Product = Common.Models.Product;

    public class ProductInformationViewModel
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, Product> products, SimplifiedProduct simpleProduct)
        {
            this.Products = products;
            this.FirstProduct = simpleProduct;

            this.Measures = new List<MeasureType>();
            foreach (var measureType in Enum.GetValues(typeof(MeasureType)))
            {
                this.Measures.Add((MeasureType)measureType);
            }
        }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public SimplifiedProduct FirstProduct { get; set; }

        public ICollection<MeasureType> Measures { get; set; }
    }
}