namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerManager manager;
        private string searchValue = string.Empty;

        public ProductsEditorViewModel(IServerManager manager)
        {
            this.manager = manager;
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
                var searchAsArray = value.ToLower().Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                this.Products.Filter = (obj) =>
                {
                    var product = obj as Product;

                    return searchAsArray.All(word => product.Name.Contains(word));
                };

                this.Products.Refresh();
            }
        }

        public bool DialogIsOpen { get; private set; }

        public ICollectionView Products => CollectionViewSource.GetDefaultView(this.manager.Cache.Products);

        public Product SelectedProduct { get; set; }

        public async void ShowProductInformationDialog()
        {
            var productInAllServers = new Dictionary<ServerInformation, Product>();
            foreach (var pair in this.manager.Cache.ProductsPerServer)
            {
                var product = pair.Value.FirstOrDefault(x => x.Name == this.SelectedProduct.Name);
                productInAllServers.Add(pair.Key, product ?? new Product());
            }
            
            var oldName = this.SelectedProduct.Name;
            this.SelectedProduct.BeginEdit();
            
            var viewModel = new ProductInformationViewModel(productInAllServers, this.SelectedProduct);
            var dialog = new ProductInformation(viewModel);
            
            this.DialogIsOpen = true;
            var result = await DialogHost.Show(dialog, "RootDialog");
            this.DialogIsOpen = false;
            
            if ((bool)result)
            {
                this.SelectedProduct.EndEdit();
                foreach (var pair in productInAllServers)
                {
                    pair.Value.NewName = this.SelectedProduct.Name;
                    pair.Value.Name = oldName;
                    pair.Value.Measure = this.SelectedProduct.Measure;
                    pair.Value.BuyingPrice = this.SelectedProduct.BuyingPrice;
                    pair.Value.Barcodes = this.SelectedProduct.Barcodes;
                    pair.Value.Quantity = pair.Value.QuantityToAdd;
                }
            
                this.manager.AddOrUpdateAsync(productInAllServers);
            }
            else
            {
                this.SelectedProduct.CancelEdit();
            }
        }
    }
}