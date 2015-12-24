namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;

    using Common.Contracts;
    using Common.Models;

    using Data.Models;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SimplifiedProduct> products;
        private readonly IServerManager manager;
        private Visibility loadingVisibility;
        private string searchValue = string.Empty;

        public ProductsEditorViewModel(IServerManager manager)
        {
            this.manager = manager;

            this.products = new ObservableCollection<SimplifiedProduct>();
            this.Products = CollectionViewSource.GetDefaultView(this.products);
            this.Products.Filter = this.Search;

            this.LoadingVisibility = Visibility.Collapsed;

            manager.ServerTestingFinished += this.OnServerTestingFinished;
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

        public SimplifiedProduct SelectedItem { get; set; }

        //TODO: Send message when dialog is opened and closed
        public async void ShowProductInformationDialog()
        {
            var productInAllServers = await this.manager.GetProductByNameAsync(this.SelectedItem.Name);

            var oldName = this.SelectedItem.Name;
            this.SelectedItem.BeginEdit();

            var viewModel = new ProductInformationViewModel(productInAllServers, this.SelectedItem);
            var dialog = new ProductInformation(viewModel);

            this.DialogIsOpen = true;
            var result = await DialogHost.Show(dialog, "RootDialog");
            this.DialogIsOpen = false;

            if ((bool)result)
            {
                this.SelectedItem.EndEdit();
                foreach (var pair in productInAllServers)
                {
                    pair.Value.Name = this.SelectedItem.Name;
                    pair.Value.OldName = oldName;
                    pair.Value.Measure = this.SelectedItem.Measure;
                    pair.Value.BuyingPrice = this.SelectedItem.BuyingPrice;
                    pair.Value.Barcodes = this.SelectedItem.Barcodes;
                    pair.Value.ExpirationDates = new List<ExpirationDate>() { new ExpirationDate() { Batch = "asdasd", Date = new DateTime(2015, 12, 29) } };
                }

                this.manager.AddOrUpdateAsync(productInAllServers);
            }
            else
            {
                this.SelectedItem.CancelEdit();
            }
        }

        private bool Search(object obj)
        {
            var product = obj as SimplifiedProduct;
            var searchAsArray = this.SearchValue.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return searchAsArray.AsParallel().All(word => product.Name.Contains(word));
        }

        private async void OnServerTestingFinished(object sender, EventArgs e)
        {
            if (this.manager.HasActiveServer)
            {
                this.LoadingVisibility = Visibility.Visible;

                this.products.Clear();

                foreach (var product in await this.manager.GetProductsByPageAsync(1, 2000))
                {
                    this.products.Add(product);
                }

                this.LoadingVisibility = Visibility.Collapsed;
            }
        }
    }
}