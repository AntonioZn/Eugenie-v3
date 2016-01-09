namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

    using Common.Helpers;
    using Common.Models;

    using GalaSoft.MvvmLight;

    public class ProductInformationViewModel : ViewModelBase
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, ProductViewModel> products, ProductViewModel mainProduct)
        {
            this.Products = products;
            this.MainProduct = mainProduct;
        }
        
        public IDictionary<ServerInformation, ProductViewModel> Products { get; set; }

        public ProductViewModel MainProduct { get; set; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();
    }
}