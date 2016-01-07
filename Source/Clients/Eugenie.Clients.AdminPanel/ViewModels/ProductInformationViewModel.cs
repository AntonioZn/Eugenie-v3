namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

    using Common.Helpers;
    using Common.Models;

    using GalaSoft.MvvmLight;

    public class ProductInformationViewModel : ViewModelBase
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, Product> products, Product mainProduct)
        {
            this.Products = products;
            this.MainProduct = mainProduct;

            this.Measures = MeasureTypeMapper.GetTypes();
        }
        
        public IDictionary<ServerInformation, Product> Products { get; set; }

        public Product MainProduct { get; set; }

        public IEnumerable<MeasureType> Measures { get; set; }
    }
}