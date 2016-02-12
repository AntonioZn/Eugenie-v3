namespace Eugenie.Clients.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;
    using Common.Notifications;
    using Common.Еxtensions;

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    public class ProductInformationViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerManager manager;
        private readonly IEnumerable<Product> products;

        public ProductInformationViewModel(IServerManager manager, Product selectedProduct, IEnumerable<Product> products)
        {
            this.manager = manager;
            this.products = products;

            this.MainProductViewModel = new ProductViewModel(selectedProduct.DeepClone());

            this.Save = new RelayCommand(this.HandleSave, this.CanSave);
            this.Cancel = new RelayCommand(this.HandleCancel);

            this.ProductInAllServers = new Dictionary<ServerInformation, ProductViewModel>();
            foreach (var pair in this.manager.Cache.ProductsPerServer)
            {
                var product = pair.Value.FirstOrDefault(x => x.Name == selectedProduct.Name);
                var productViewModel = new ProductViewModel(product ?? new Product());
                this.ProductInAllServers.Add(pair.Key, productViewModel);
            }
        }

        public ICommand Cancel { get; set; }

        public ICommand Save { get; set; }

        public IDictionary<ServerInformation, ProductViewModel> ProductInAllServers { get; set; }

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
            foreach (var pair in this.ProductInAllServers)
            {
                pair.Value.MapProperties(this.MainProductViewModel);
                this.manager.AddOrUpdate(pair.Key.Name, pair.Value.GetModel());
            }
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private bool CanSave()
        {
            var exists = this.manager.Cache.ProductsPerServer.FirstOrDefault(x => x.Value.Any()).Value?
                             .Any(y => y.Name.ToLower() == this.MainProductViewModel.Product.Name.ToLower() && y.Name.ToLower() != this.MainProductViewModel.OldName.ToLower());

            return this.MainProductViewModel.Product.HasNoValidationErrors()
                   && this.MainProductViewModel.HasNoValidationErrors()
                   && this.ProductInAllServers.Values.All(x => x.HasNoValidationErrors())
                   && !exists.GetValueOrDefault();
        }
    }
}