namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Http;
    using System.Windows;
    using System.Windows.Data;

    using Common.Contracts;
    using Common.Models;
    using Common.Helpers;
    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SimplifiedProduct> products;
        private readonly IServersManager serversManager;
        private readonly IWebApiServerClient client;
        private Visibility loadingVisibility;
        private SimplifiedProduct selectedItem;
        private string searchValue = string.Empty;
        private IDictionary<ServerInformation, HttpClient> activeServers;

        public ProductsEditorViewModel(IServersManager serversManager, IWebApiServerClient client)
        {
            this.client = client;
            this.serversManager = serversManager;
            this.serversManager.ServerTestingFinished += this.OnServerTestingFinished;

            this.products = new ObservableCollection<SimplifiedProduct>();
            this.Products = CollectionViewSource.GetDefaultView(this.products);
            this.Products.Filter = this.Search;

            this.LoadingVisibility = Visibility.Collapsed;
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

        //TODO: Send message when dialog is opened and closed
        public async void ShowProductInformationDialog()
        {
            var productInAllServers = await this.client.GetProductByIdAsync(this.activeServers, this.selectedItem.Id);

            this.selectedItem.BeginEdit();

            var viewModel = new ProductInformationViewModel(productInAllServers, this.selectedItem);
            var dialog = new ProductInformation(viewModel);

            this.DialogIsOpen = true;
            var result = await DialogHost.Show(dialog, "RootDialog");
            this.DialogIsOpen = false;

            if ((bool)result)
            {
                this.selectedItem.EndEdit();
                foreach (var pair in productInAllServers)
                {
                    pair.Value.Name = this.selectedItem.Name;
                    pair.Value.Measure = this.selectedItem.Measure;
                    pair.Value.BuyingPrice = this.selectedItem.BuyingPrice;
                    pair.Value.Barcodes = this.selectedItem.Barcodes.ToList();
                }

                this.client.UpdateAsync(this.activeServers, productInAllServers);
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

        private async void OnServerTestingFinished(object sender, ServersManager.ServerTestingFinishedEventArgs e)
        {
            this.activeServers = e.ActiveServers;

            this.LoadingVisibility = Visibility.Visible;

            this.products.Clear();
            if (this.activeServers.Any())
            {
                foreach (var product in await this.client.GetProductsByPageAsync(this.activeServers.First(), 1, 2000))
                {
                    this.products.Add(product);
                }
            }

            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}