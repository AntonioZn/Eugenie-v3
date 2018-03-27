namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    
    using Common;
    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.WebApiModels;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Mvvm;

    using Views;

    using LotteryTicketChecker = Helpers.LotteryTicketChecker;

    public class SellViewModel : ViewModelBase, IBarcodeHandler, IKeyHandler
    {
        private readonly INavigationService navigationService;
        private readonly FiscalPrinterHandler fiscalPrinterHandler;
        private readonly LotteryTicketChecker lotteryTicketChecker;
        private string fullname;
        private StoreClient client;

        public SellViewModel(INavigationService navigationService, FiscalPrinterHandler fiscalPrinterHandler, LotteryTicketChecker lotteryTicketChecker)
        {
            this.navigationService = navigationService;
            this.fiscalPrinterHandler = fiscalPrinterHandler;
            this.lotteryTicketChecker = lotteryTicketChecker;

            this.Basket = new BasketViewModel();
        }

        public string FullName
        {
            get => this.fullname;
            set => this.Set(ref this.fullname, value);
        }

        public BasketViewModel Basket { get; }

        public Product SelectedProduct { get; set; }

        public async void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);

            var product = await this.client.GetProductByBarcodeAsync(barcode);
            if (product != null)
            {
                this.AddToBasket(product);
            }
            else
            {
                await this.navigationService.ShowDialogAsync<MissingProductViewModel>();
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Delete:
                    this.RemoveSelectedProductFromBasket();
                    e.Handled = true;
                    break;
                case Key.Enter:
                    this.ChangeProductQuantity();
                    e.Handled = true;
                    break;
                case Key.F1:
                    this.SearchProducts();
                    e.Handled = true;
                    break;
                case Key.F3:
                    this.CheckTicket();
                    e.Handled = true;
                    break;
                case Key.F5:
                    this.Logout();
                    e.Handled = true;
                    break;
                case Key.F10:
                    this.WasteProducts();
                    e.Handled = true;
                    break;
                case Key.F11:
                    this.SellProducts(true);
                    e.Handled = true;
                    break;
                case Key.F12:
                    this.SellProducts(false);
                    e.Handled = true;
                    break;
            }
        }

        private async void CheckTicket()
        {
            if (this.lotteryTicketChecker.IsLoggedIn)
            {
                var result = await this.navigationService.ShowDialogAsync<LotteryTicketCheckerViewModel, bool>();
                if (result)
                {
                    
                }
            }
        }

        private async void RemoveSelectedProductFromBasket()
        {
            if (this.SelectedProduct != null)
            {
                var result = await ConfirmationsHost.Show("Потвърди", $"Изтриване на {this.SelectedProduct.Name}?", "RootDialog");
                if (result)
                {
                    this.Basket.Delete(this.SelectedProduct);
                }
            }
            else
            {
                NotificationsHost.Error("Notifications", "Изберете продукт", "Трябва да има маркиран продукт.");
            }
        }

        private void ChangeProductQuantity()
        {
            if (this.SelectedProduct != null)
            {
                this.AddToBasket(this.SelectedProduct);
            }
            else
            {
                NotificationsHost.Error("Notifications", "Изберете продукт", "Трябва да има маркиран продукт.");
            }
        }

        private async void SearchProducts()
        {
            var result = await this.navigationService.ShowDialogAsync<ProductsSearchViewModel, Product>(this.client);
            if (result != null)
            {
                this.AddToBasket(result);
            }
        }

        private async void WasteProducts()
        {
            if (!this.Basket.Products.Any())
            {
                NotificationsHost.Error("Notifications", "Добавете продукт", "В списъка трябва да има поне 1 продукт.");
                return;
            }

            var result = await ConfirmationsHost.Show("Потвърди", "Бракуване?", "RootDialog");
            if (result)
            {
                var pairs = this.Basket.Products.Select(x => new IdQuantityPair
                                                             {
                                                                 Id = x.Id,
                                                                 Quantity = x.Quantity.Value
                                                             });
                await this.client.WasteProductsAsync(pairs);
                this.Basket.Clear();
            }
        }

        private async void SellProducts(bool printReceipt)
        {
            if (!this.Basket.Products.Any())
            {
                NotificationsHost.Error("Notifications", "Добавете продукт", "В списъка трябва да има поне 1 продукт.");
                return;
            }

            var result = await this.navigationService.ShowDialogAsync<ChangeCalculatorViewModel, bool>(this.Basket.TotalPrice);
            if (result)
            {
                var pairs = this.Basket.Products.Select(x => new IdQuantityPair
                                                             {
                                                                 Id = x.Id,
                                                                 Quantity = x.Quantity.Value
                                                             });
                await this.client.SellProductsAsync(pairs);
                if (printReceipt)
                {
                    this.fiscalPrinterHandler.ExportReceipt(this.Basket.Products);
                }

                this.Basket.Clear();
            }
        }

        private async void Logout()
        {
            var result = await ConfirmationsHost.Show("Потвърди", "Излизане?", "RootDialog");
            if (result)
            {
                await this.navigationService.NavigateToAsync<LoginViewModel>();
            }
        }

        private async void AddToBasket(Product product)
        {
            decimal startingQuantity = 1;
            var existingProduct = this.Basket.Products.FirstOrDefault(x => x.Id == product.Id);
            if (existingProduct != null)
            {
                startingQuantity = existingProduct.Quantity.Value + 1;
            }

            decimal maximumQuantity = 1000;
            if (product.Name.Contains("$"))
            {
                maximumQuantity = this.Basket.Products.Where(x => x.Name.StartsWith("хляб") && x.SellingPrice >= product.SellingPrice * -1).Sum(x => x.Quantity).GetValueOrDefault();
            }

            if (maximumQuantity > 0)
            {
                var quantityEditorViewModel = new QuantityEditorViewModel(startingQuantity, maximumQuantity, product.Measure, product.Name);
                var quantityEditor = new QuantityEditor(quantityEditorViewModel);
                var result = await DialogHost.Show(quantityEditor, "RootDialog");

                if ((bool) result)
                {
                    product.Quantity = decimal.Parse(quantityEditorViewModel.Quantity);
                    this.Basket.Add(product);
                }
            }
            else
            {
                NotificationsHost.Error("Notifications", "Забранено", "Продуктът не може да бъде добавен.");
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            this.client = (StoreClient) navigationData;
            var userInfo = await this.client.GetUserInfoAsync();
            this.FullName = $"{userInfo.FirstName} {userInfo.LastName}";
        }
    }
}