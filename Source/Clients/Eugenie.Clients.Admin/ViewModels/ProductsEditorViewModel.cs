namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;
    using Common.Notifications;
    using Common.Views;
    using Common.Еxtensions;

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase, IBarcodeHandler, IKeyHandler
    {
        private readonly IServerManager manager;
        private readonly ObservableCollection<Product> products = new ObservableCollection<Product>();
        private ProductInformationViewModel productInformationViewModel;
        private string searchValue = string.Empty;

        public ProductsEditorViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
            this.manager.ProductsCacheChanged += this.OnServerTestingFinished;

            this.Enter = new RelayCommand(this.HandleEnter);
        }

        public ICommand Enter { get; }

        public string SearchValue
        {
            get
            {
                return this.searchValue;
            }

            set
            {
                this.Set(() => this.SearchValue, ref this.searchValue, value);
                var searchAsArray = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                this.Products.Filter = obj =>
                                       {
                                           var product = obj as Product;

                                           return searchAsArray.All(word => product.Name.Contains(word));
                                       };

                this.Products.Refresh();
            }
        }

        public ICollectionView Products => CollectionViewSource.GetDefaultView(this.products);

        public Product SelectedProduct { get; set; }

        public void HandleBarcode(string barcode)
        {
            if (this.productInformationViewModel != null)
            {
                var existingProduct = ExistingBarcodeChecker.Check(barcode, this.productInformationViewModel.MainProductViewModel.Product, this.products);
                if (existingProduct != null)
                {
                    NotificationsHost.Error("Баркодът съществува", $"\"{existingProduct.Name}\" съдържа този баркод.");
                    return;
                }
                this.productInformationViewModel.HandleBarcode(barcode);
                return;
            }

            this.SearchValue = string.Empty;

            this.Products.Filter = obj =>
                                   {
                                       var product = obj as Product;

                                       return product.Barcodes.Any(x => x.Value == barcode);
                                   };

            this.Products.Refresh();
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    this.HandleEnter();
                    e.Handled = true;
                    break;
                case Key.Escape:
                    this.HandleEscape();
                    e.Handled = true;
                    break;
                case Key.Delete:
                    this.HandleDelete();
                    e.Handled = true;
                    break;
            }
        }

        public void HandleEscape()
        {
            this.SearchValue = string.Empty;
        }

        public async void HandleEnter()
        {
            if (this.SelectedProduct == null)
            {
                return;
            }

            this.productInformationViewModel = new ProductInformationViewModel(this.manager, this.SelectedProduct);
            var dialog = new ProductInformation(this.productInformationViewModel);

            var result = await DialogHost.Show(dialog, "RootDialog");
            this.productInformationViewModel = null;

            if ((bool) result)
            {
                this.Products.Refresh();
            }
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.products.Clear();
            this.manager.Cache.ProductsPerServer.FirstOrDefault(x => x.Value.Any()).Value.ForEach(this.products.Add);
        }

        private async void HandleDelete()
        {
            if (this.SelectedProduct == null)
            {
                return;
            }

            var result = await DialogHost.Show(new Confirm($"Изтриване на {this.SelectedProduct.Name}?"), "RootDialog");
            if ((bool) result)
            {
                this.manager.Delete(this.SelectedProduct.Name);
            }
        }
    }
}