namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Autofac;

    using Common.Contracts;
    using Common.Models;
    using Common.Views;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase, IBarcodeHandler, IKeyHandler
    {
        private readonly ServerManager manager;
        private readonly ObservableCollection<Product> products = new ObservableCollection<Product>();
        private readonly DispatcherTimer timer;
        private string search = string.Empty;

        public ProductsEditorViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
            this.manager.ProductsCacheChanged += this.OnServerTestingFinished;

            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(0.5);
            this.timer.Tick += this.HandleSearch;

            this.Enter = new RelayCommand(this.OpenInformation);
        }

        public ICommand Enter { get; }

        public ICollectionView Products => CollectionViewSource.GetDefaultView(this.products);

        public Product SelectedProduct { get; set; }

        public string Search
        {
            get => this.search;
            set
            {
                this.Set(() => this.Search, ref this.search, value);
                if (this.Search != null)
                {
                    this.timer.Stop();
                    this.timer.Start();
                }
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    this.OpenInformation();
                    e.Handled = true;
                    break;
                case Key.Escape:
                    this.ClearSearch();
                    e.Handled = true;
                    break;
                case Key.Delete:
                    this.HandleDelete();
                    e.Handled = true;
                    break;
            }
        }

        public void HandleBarcode(string barcode)
        {
            this.Search = null;

            this.Products.Filter = obj =>
            {
                var product = obj as Product;

                return product.Barcodes.Any(x => x.Value == barcode);
            };

            this.Products.Refresh();

            var productWithBarcode = this.products.FirstOrDefault(x => x.Barcodes.Any(y => y.Value == barcode));
            if (productWithBarcode != null)
            {
                this.SelectedProduct = productWithBarcode;
                this.OpenInformation();
            }
            else
            {
                ViewModelLocator.Container.Resolve<DeliveryViewModel>().ImportMissingProduct(string.Empty, barcode);
                ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowDelivery();
            }
        }

        public void ClearSearch()
        {
            this.Search = null;
            this.Products.Filter = null;
            this.Products.Refresh();
        }

        public async void OpenInformation()
        {
            if (this.SelectedProduct == null)
            {
                return;
            }

            var productInformationViewModel = new ProductInformationViewModel(this.manager, this.SelectedProduct, this.products);
            var dialog = new ProductInformation(productInformationViewModel);

            var result = await DialogHost.Show(dialog, "RootDialog");

            if ((bool)result)
            {
                this.Products.Refresh();
            }
        }

        private void HandleSearch(object sender, EventArgs e)
        {
            this.timer.Stop();
            var searchAsArray = this.Search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            this.Products.Filter = obj =>
            {
                var product = obj as Product;

                return searchAsArray.All(word => product.Name.Contains(word));
            };

            this.Products.Refresh();
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

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.products.Clear();
            this.manager.Stores.OrderByDescending(x => x.Products.Count).FirstOrDefault()?.Products.ForEach(this.products.Add);
        }
    }
}