namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Common.Contracts;
    using Common.Messages;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase
    {
        private HashSet<SimplifiedProduct> allProducts;
        private ObservableCollection<SimplifiedProduct> products;
        private readonly IWebApiServerClient client;
        private Visibility loadingVisibility;
        private SimplifiedProduct selectedItem;
        private string searchValue = string.Empty;

        public ProductsEditorViewModel(IWebApiServerClient client)
        {
            this.client = client;
            this.LoadingVisibility = Visibility.Collapsed;
            Messenger.Default.Register<ServerTestingFinishedMessage>(this, this.OnServerTestingFinishedMessage);
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
                this.Search();
            }
        }

        public bool DialogIsOpen { get; private set; }

        public Visibility LoadingVisibility
        {
            get
            {
                return this.loadingVisibility;
            }

            set
            {
                this.Set(() => this.LoadingVisibility, ref this.loadingVisibility, value);
            }
        }

        public IEnumerable<SimplifiedProduct> Products
        {
            get
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<SimplifiedProduct>();
                }

                return this.products;
            }

            set
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<SimplifiedProduct>();
                }

                this.products.Clear();
                foreach (var product in value)
                {
                    this.products.Add(product);
                }
            }
        }

        public SimplifiedProduct SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                this.Set(() => this.SelectedItem, ref this.selectedItem, value);
            }
        }

        public async void ShowProductInformationDialog()
        {
            var productInAllServers = await this.client.GetProductByIdAsync(this.selectedItem.Id);
            var simpleProduct = new SimplifiedProduct(this.selectedItem.Id, this.selectedItem.Name, this.selectedItem.BuyingPrice, this.selectedItem.Measure, this.selectedItem.Barcodes);
            var viewModel = new ProductInformationViewModel(productInAllServers, simpleProduct);
            var dialog = new ProductInformation(viewModel);

            this.DialogIsOpen = true;
            //TODO: Send message when dialog is opened and closed
            var result = await DialogHost.Show(dialog, "RootDialog");
            this.DialogIsOpen = false;

            if ((bool)result)
            {
                foreach (var pair in productInAllServers)
                {
                    pair.Value.Name = simpleProduct.Name;
                    pair.Value.Measure = simpleProduct.Measure;
                    pair.Value.BuyingPrice = simpleProduct.BuyingPrice;
                    pair.Value.Barcodes = simpleProduct.Barcodes.ToList();
                }

                //TODO: update product in Products list
                this.client.UpdateAsync(productInAllServers);
            }
        }

        private void Search()
        {
            var searchAsArray = this.SearchValue.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            this.Products = this.allProducts.Where(x => searchAsArray.All(n => x.Name.ToLower().Contains(n))).OrderByDescending(x => searchAsArray.Any(n => x.Name.StartsWith(n))).ToList();
        }


        private async void OnServerTestingFinishedMessage(ServerTestingFinishedMessage obj)
        {
            this.LoadingVisibility = Visibility.Visible;
            var responseProducts = await this.client.GetProductsByPageAsync(1, 2000);
            this.allProducts = new HashSet<SimplifiedProduct>(responseProducts);
            this.Products = this.allProducts;
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}