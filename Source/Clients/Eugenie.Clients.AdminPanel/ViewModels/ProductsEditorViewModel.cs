namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    using Common.Contracts;
    using Common.Messages;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SimplifiedProduct> products;
        private readonly IWebApiServerClient client;
        private Visibility loadingVisibility;
        private SimplifiedProduct selectedItem;
        private string searchValue = string.Empty;

        public ProductsEditorViewModel(IWebApiServerClient client)
        {
            this.client = client;

            this.products = new ObservableCollection<SimplifiedProduct>();
            this.Products = CollectionViewSource.GetDefaultView(this.products);
            this.Products.Filter = this.Search;

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
                this.Products.Refresh();
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

        public ICollectionView Products { get; set; }

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
            this.selectedItem.BeginEdit();
            var viewModel = new ProductInformationViewModel(productInAllServers, this.selectedItem);
            var dialog = new ProductInformation(viewModel);

            this.DialogIsOpen = true;
            //TODO: Send message when dialog is opened and closed
            var result = await DialogHost.Show(dialog, "RootDialog");
            this.DialogIsOpen = false;

            if ((bool) result)
            {
                this.selectedItem.EndEdit();
                foreach (var pair in productInAllServers)
                {
                    pair.Value.Name = this.selectedItem.Name;
                    pair.Value.Measure = this.selectedItem.Measure;
                    pair.Value.BuyingPrice = this.selectedItem.BuyingPrice;
                    pair.Value.Barcodes = this.selectedItem.Barcodes.ToList();
                }
                
                this.client.UpdateAsync(productInAllServers);
            }
            else
            {
                this.selectedItem.CancelEdit();
            }
        }

        private bool Search(object obj)
        {
            var product = obj as SimplifiedProduct;
            var searchAsArray = this.SearchValue.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return searchAsArray.All(word => product.Name.Contains(word));
        }
        
        private async void OnServerTestingFinishedMessage(ServerTestingFinishedMessage obj)
        {
            this.LoadingVisibility = Visibility.Visible;
            this.products.Clear();
            foreach (var product in await this.client.GetProductsByPageAsync(1, 2000))
            {
                this.products.Add(product);
            }
            
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}