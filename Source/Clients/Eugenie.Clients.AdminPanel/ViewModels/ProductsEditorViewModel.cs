namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using Common.Contracts;
    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    using Notifications;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase, IBarcodeHandler, IEnterHandler
    {
        private readonly IServerManager manager;
        private readonly ObservableCollection<Product> products = new ObservableCollection<Product>();
        private string searchValue = string.Empty;

        public ProductsEditorViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
        }

        public void HandleBarcode(string barcode)
        {
            this.SearchValue = string.Empty;

            this.Products.Filter = (obj) =>
            {
                var product = obj as Product;

                return product.Barcodes.Any(x => x.Value == barcode);
            };

            this.Products.Refresh();
        }

        public string SearchValue
        {
            get
            {
                return this.searchValue;
            }

            set
            {
                this.Set(() => this.SearchValue, ref this.searchValue, value);
                var searchAsArray = value.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                this.Products.Filter = (obj) =>
                {
                    var product = obj as Product;

                    return searchAsArray.All(word => product.Name.Contains(word));
                };

                this.Products.Refresh();
            }
        }

        public ICollectionView Products => CollectionViewSource.GetDefaultView(this.products);

        public Product SelectedProduct { get; set; }

        public async void HandleEnter()
        {
            var productInAllServers = new Dictionary<ServerInformation, ProductViewModel>();
            foreach (var pair in this.manager.Cache.ProductsPerServer)
            {
                var product = pair.Value.FirstOrDefault(x => x.Name == this.SelectedProduct.Name);
                var productViewModel = new ProductViewModel(product ?? new Product());
                productInAllServers.Add(pair.Key, productViewModel);
            }

            var selectedProductViewModel = new ProductViewModel(this.SelectedProduct.DeepClone());
            var viewModel = new ProductInformationViewModel(productInAllServers, selectedProductViewModel);
            var dialog = new ProductInformation(viewModel);
            
            var result = await DialogHost.Show(dialog, "RootDialog");

            if ((bool)result)
            {
                foreach (var pair in productInAllServers)
                {
                    pair.Value.MapProperties(selectedProductViewModel);
                    this.manager.AddOrUpdate(pair.Key, pair.Value.GetModel());
                }

                this.Products.Refresh();
            }
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.products.Clear();
            this.manager.Cache.ProductsPerServer.FirstOrDefault(x => x.Value.Any()).Value.ForEach(this.products.Add);
        }
    }
}