namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Extensions;
    using Sv.Wpf.Core.Helpers;
    using Sv.Wpf.Core.Mvvm;

    public class ProductsSearchViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly TaskManager taskManager;
        private StoreClient client;
        private string search;
        private TaskManager.Task searchTask;

        public ProductsSearchViewModel(TaskManager taskManager)
        {
            this.taskManager = taskManager;
        }

        public ICommand SelectProductCommand => new RelayCommand(this.SelectProduct);

        public Product SelectedProduct { get; set; }

        public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }

        public override Task InitializeAsync(object navigationData)
        {
            this.client = (StoreClient) navigationData;
            return Task.CompletedTask;
        }

        public string Search
        {
            get => this.search;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.Products.Clear();
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
            if (this.searchTask != null)
            {
                this.taskManager.CancelTask(this.searchTask);
            }

            this.searchTask = new TaskManager.Task("search", false);
            this.searchTask.Function = async (cts, logger) =>
                                       {
                                           var product = await this.client.GetProductByIdAsync(id, cts.Token);
                                           this.Products.Clear();

                                           if (product != null)
                                           {
                                               this.Products.Add(product);
                                           }
                                       };

            await this.taskManager.Run(this.searchTask);
        }

        private async void SearchByName()
        {
            if (this.searchTask != null)
            {
                this.taskManager.CancelTask(this.searchTask);
            }

            this.searchTask = new TaskManager.Task("search", false);
            this.searchTask.Function = async (cts, logger) =>
                                       {
                                           var responseProducts = await this.client.GetProductsByNameAsync(this.Search, cts.Token);
                                           this.Products.Clear();
                                           this.Products.AddRange(responseProducts);
                                       };

            await this.taskManager.Run(this.searchTask);
        }

        private void SelectProduct()
        {
            if (this.Products.Any())
            {
                this.SelectedProduct = this.SelectedProduct ?? this.Products.FirstOrDefault();
                DialogHost.CloseDialogCommand.Execute(this.SelectedProduct, null);
            }
        }
    }
}