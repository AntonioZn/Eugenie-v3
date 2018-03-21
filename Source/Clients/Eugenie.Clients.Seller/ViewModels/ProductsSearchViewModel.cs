namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Extensions;
    using Sv.Wpf.Core.Mvvm;

    public class ProductsSearchViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly StoreClient client;
        private ObservableCollection<Product> products;
        private string search;
        private CancellationTokenSource cts = new CancellationTokenSource();

        public ProductsSearchViewModel(StoreClient client)
        {
            this.client = client;
        }

        public ICommand Add => new RelayCommand(this.HandleAdd);

        public Product SelectedProduct { get; set; }

        public IEnumerable<Product> Products
        {
            get => this.products ?? (this.products = new ObservableCollection<Product>());

            set
            {
                this.products = this.products ?? new ObservableCollection<Product>();
                this.products.Clear();
                value.ForEach(this.products.Add);
            }
        }

        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }

        public string Search
        {
            get => this.search;

            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    this.products.Clear();
                    this.search = "";
                    return;
                }

                if (int.TryParse(value, out var id))
                {
                    this.search = value;
                    this.SearchById(id);
                    return;
                }

                if (this.search != value.Trim())
                {
                    this.search = value.TrimMultipleWhiteSpaces();
                    this.SearchByName();
                }

                this.search = value.TrimMultipleWhiteSpaces();
            }
        }

        private async void SearchById(int id)
        {
            var product = await this.client.GetProductByIdAsync(id);
            this.products.Clear();
            
            if (product != null)
            {
                this.products.Add(product);
            }
        }

        //TODO: use task manager
        private async void SearchByName()
        {
            try
            {
                this.cts.Cancel();
                this.cts = new CancellationTokenSource();
                var responseProducts = await this.client.GetProductsByNameAsync(this.Search, this.cts.Token);
                this.Products = responseProducts;
            }
            catch
            {
                
            }
        }

        private void HandleAdd()
        {
            if (this.Products.Any())
            {
                this.SelectedProduct = this.SelectedProduct ?? this.Products.FirstOrDefault();

                DialogHost.CloseDialogCommand.Execute(true, null);
            }
        }
    }
}