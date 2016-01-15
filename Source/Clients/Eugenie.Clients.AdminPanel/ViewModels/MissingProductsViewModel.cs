namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

    using Autofac;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class MissingProductsViewModel : ViewModelBase, IDeleteHandler, IEnterHandler
    {
        private readonly IServerManager manager;

        public MissingProductsViewModel(IServerManager manager)
        {
            this.manager = manager;
        }

        public MissingProduct SelectedProduct { get; set; }

        public IEnumerable<MissingProduct> MissingProducts => this.manager.Cache.MissingProducts;
        
        public void HandleDelete()
        {
            //throw new System.NotImplementedException();
        }

        public async void HandleEnter()
        {
            var viewModel = ViewModelLocator.container.Resolve<DeliveryViewModel>();
            viewModel.Name = this.SelectedProduct.Name;
            viewModel.MainProductViewModel.Product.Barcodes.Add(new Barcode(this.SelectedProduct.Barcode));
            var dialog = new Delivery(true);
            await DialogHost.Show(dialog, "RootDialog");
        }
    }
}
