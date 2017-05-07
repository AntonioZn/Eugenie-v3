namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;
    using Common.Notifications;
    using Common.Еxtensions;

    using Contracts;
    
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Models;

    public class ProductInformationViewModel : IBarcodeHandler
    {
        private readonly ServerManager manager;
        private readonly IEnumerable<Product> products;

        public ProductInformationViewModel(ServerManager manager, Product selectedProduct, IEnumerable<Product> products)
        {
            this.manager = manager;
            this.products = products;

            //TODO: Implement DeepClone method in product
            this.MainProductViewModel = new ProductViewModel(selectedProduct.DeepClone());

            this.Save = new RelayCommand(this.HandleSave, this.CanSave);
            this.Cancel = new RelayCommand(this.HandleCancel);

            this.ProductInAllServers = new Dictionary<Store, ProductViewModel>();
            foreach (var server in this.manager.Stores)
            {
                var product = server.Products.FirstOrDefault(x => x.Name == selectedProduct.Name);
                var productViewModel = new ProductViewModel(product ?? new Product());
                this.ProductInAllServers.Add(server, productViewModel);
            }
        }

        public ICommand Cancel { get; set; }

        public ICommand Save { get; set; }

        public IDictionary<Store, ProductViewModel> ProductInAllServers { get; set; }

        public ProductViewModel MainProductViewModel { get; set; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        public void HandleBarcode(string barcode)
        {
            var existingProduct = ExistingBarcodeChecker.Check(barcode, this.MainProductViewModel.Product, this.products);
            if (existingProduct != null)
            {
                NotificationsHost.Error("Баркодът съществува", $"\"{existingProduct.Name}\" съдържа този баркод.");
            }
            else
            {
                this.MainProductViewModel.Product.Barcodes.Add(new Barcode(barcode));
            }
        }

        private void HandleCancel()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private void HandleSave()
        {
            this.manager.AddOrUpdate(this.ProductInAllServers, this.MainProductViewModel);
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private bool CanSave()
        {
            var exists = this.products.Any(y => string.Equals(y.Name, this.MainProductViewModel.Product.Name, StringComparison.CurrentCultureIgnoreCase)
                                                && !string.Equals(y.Name, this.MainProductViewModel.OldName, StringComparison.CurrentCultureIgnoreCase));

            return this.MainProductViewModel.Product.HasNoValidationErrors()
                   && this.MainProductViewModel.HasNoValidationErrors()
                   && this.ProductInAllServers.Values.All(x => x.HasNoValidationErrors())
                   && !exists;
        }
    }
}