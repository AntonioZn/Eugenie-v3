namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Linq;

    using Common.Models;

    using Data.Models;

    using GalaSoft.MvvmLight;

    public class ProductInformationViewModel : ViewModelBase
    {
        public ProductInformationViewModel(SimplifiedProduct simpleProduct)
        {
            this.Product = new Product
                           {
                               Id = simpleProduct.Id,
                               Name = simpleProduct.Name,
                               Measure = simpleProduct.Measure,
                               BuyingPrice = simpleProduct.BuyingPrice,
                               Barcodes = simpleProduct.Barcodes.ToList()
                           };
        }

        public Product Product { get; set; }
    }
}