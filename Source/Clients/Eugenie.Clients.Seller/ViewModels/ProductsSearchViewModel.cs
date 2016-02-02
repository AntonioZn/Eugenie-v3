namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MaterialDesignThemes.Wpf;

    public class ProductsSearchViewModel : ViewModelBase
    {
        private readonly IWebApiClient apiClient;
        private string search;
        private ObservableCollection<Product> products;

        public ProductsSearchViewModel(IWebApiClient apiClient)
        {
            this.apiClient = apiClient;

            this.Add = new RelayCommand(this.HandleAdd);
            this.Cancel = new RelayCommand(this.HandleCancel);
        }

        public ICommand Add { get; }

        public ICommand Cancel { get; }

        public Product SelectedProduct { get; set; }

        public IEnumerable<Product> Products
        {
            get
            {
                return this.products ?? (this.products = new ObservableCollection<Product>());
            }

            set
            {
                this.products = this.products ?? new ObservableCollection<Product>();
                this.products.Clear();
                value.ForEach(this.products.Add);
            }
        }

        public string Search
        {
            get
            {
                return this.search;
            }

            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    this.products.Clear();
                    this.search = "";
                    return;
                }

                if (this.search != value.Trim())
                {
                    this.search = value.RemoveMultipleWhiteSpaces();
                    this.HandleSearch();
                }

                this.search = value.RemoveMultipleWhiteSpaces();
            }
        }

        private async void HandleSearch()
        {
            var responseProducts = await this.apiClient.GetProductsByNameAsync(ViewModelLocator.httpClient, this.Search);
            this.Products = responseProducts;
        }

        private void HandleAdd()
        {
            if (this.SelectedProduct != null)
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }

        private void HandleCancel()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }
    }
}