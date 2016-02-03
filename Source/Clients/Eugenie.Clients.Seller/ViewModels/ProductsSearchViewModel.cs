﻿namespace Eugenie.Clients.Seller.ViewModels
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

                int id;
                if (int.TryParse(value, out id))
                {
                    this.search = value;
                    this.SearchById(id);
                    return;
                }

                if (this.search != value.Trim())
                {
                    this.search = value.RemoveMultipleWhiteSpaces();
                    this.SearchByName();
                }

                this.search = value.RemoveMultipleWhiteSpaces();
            }
        }

        private async void SearchById(int id)
        {
            var product = await this.apiClient.GetProductById(ViewModelLocator.httpClient, id);
            this.products.Clear();;
            if (product != null)
            {
                this.products.Add(product);
            }
        }

        private async void SearchByName()
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