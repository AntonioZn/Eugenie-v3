﻿namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using Common.Contracts;
    using Common.Helpers;
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
                var searchAsArray = value.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                this.Products.Filter = (obj) =>
                {
                    var product = obj as Product;

                    return searchAsArray.All(word => product.Name.Contains(word));
                };

                this.Products.Refresh();
            }
        }

        public ICollectionView Products => CollectionViewSource.GetDefaultView(this.manager.Cache.Products);

        public Product SelectedProduct { get; set; }

        public async void ShowProductInformationDialog()
        {
            var productInAllServers = new Dictionary<ServerInformation, ProductViewModel>();
            foreach (var pair in this.manager.Cache.ProductsPerServer)
            {
                var product = pair.Value.FirstOrDefault(x => x.Name == this.SelectedProduct.Name);
                var productViewModel = new ProductViewModel(product ?? new Product());
                productInAllServers.Add(pair.Key, productViewModel);
            }

            var selectedProductViewModel = new ProductViewModel(DeepCopier<Product>.Copy(this.SelectedProduct));
            var viewModel = new ProductInformationViewModel(productInAllServers, selectedProductViewModel);
            var dialog = new ProductInformation(viewModel);
            
            var result = await DialogHost.Show(dialog, "RootDialog");
            
            if ((bool) result)
            {
                foreach (var pair in productInAllServers)
                {
                    pair.Value.MapProperties(selectedProductViewModel);
                }
            
                await this.manager.AddOrUpdateAsync(productInAllServers);
            
                this.Products.Refresh();
            }
        }
    }
}