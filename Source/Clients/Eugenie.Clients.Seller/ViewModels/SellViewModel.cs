namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Models;
    using Common.Notifications;
    using Common.Views;
    using Common.WebApiModels;

    using GalaSoft.MvvmLight;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Views;

    using LotteryTicketChecker = Helpers.LotteryTicketChecker;
    using MissingProduct = Views.MissingProduct;

    public class SellViewModel : ViewModelBase, IBarcodeHandler, IKeyHandler
    {
        private readonly IWebApiClient apiClient;
        private readonly FiscalPrinterHandler fiscalPrinterHandler;
        private readonly LotteryTicketChecker lotteryTicketChecker;
        private string fullname;

        public SellViewModel(IWebApiClient apiClient, FiscalPrinterHandler fiscalPrinterHandler, LotteryTicketChecker lotteryTicketChecker)
        {
            this.apiClient = apiClient;
            this.fiscalPrinterHandler = fiscalPrinterHandler;
            this.lotteryTicketChecker = lotteryTicketChecker;

            this.Basket = new BasketViewModel();

            this.Initialize();
        }

        public string FullName
        {
            get
            {
                return this.fullname;
            }

            set
            {
                this.Set(() => this.FullName, ref this.fullname, value);
            }
        }

        public BasketViewModel Basket { get; }

        public Product SelectedProduct { get; set; }

        public async void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);

            var product = await this.apiClient.GetProductByBarcode(ViewModelLocator.httpClient, barcode);
            if (product != null)
            {
                this.AddToBasket(product);
            }
            else
            {
                await DialogHost.Show(new MissingProduct(), "RootDialog");
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Delete:
                    this.HandleDelete();
                    e.Handled = true;
                    break;
                case Key.Enter:
                    this.HandleEnter();
                    e.Handled = true;
                    break;
                case Key.F1:
                    this.HandleF1();
                    e.Handled = true;
                    break;
                case Key.F3:
                    this.HandleCheckTicket();
                    e.Handled = true;
                    break;
                case Key.F5:
                    this.HandleF5();
                    e.Handled = true;
                    break;
                case Key.F10:
                    this.HandleF10();
                    e.Handled = true;
                    break;
                case Key.F11:
                    this.HandleF11();
                    e.Handled = true;
                    break;
                case Key.F12:
                    this.HandleF12();
                    e.Handled = true;
                    break;
            }
        }

        private async void HandleCheckTicket()
        {
            if (this.lotteryTicketChecker.IsLoggedIn)
            {
                var viewModel = new LotteryTicketCheckerViewModel(this.lotteryTicketChecker);
                var view = new Views.LotteryTicketChecker(viewModel);

                var result = await DialogHost.Show(view, "RootDialog");
                if ((bool) result)
                {
                    
                }
            }
        }

        public async void HandleDelete()
        {
            if (this.SelectedProduct != null)
            {
                var result = await DialogHost.Show(new Confirm($"Изтриване на {this.SelectedProduct.Name}?"), "RootDialog");
                if ((bool) result)
                {
                    this.Basket.Delete(this.SelectedProduct);
                }
            }
            else
            {
                NotificationsHost.Error("Изберете продукт", "Трябва да има маркиран продукт.");
            }
        }

        public void HandleEnter()
        {
            if (this.SelectedProduct != null)
            {
                this.AddToBasket(this.SelectedProduct);
            }
            else
            {
                NotificationsHost.Error("Изберете продукт", "Трябва да има маркиран продукт.");
            }
        }

        public async void HandleF1()
        {
            var productsSearchViewModel = new ProductsSearchViewModel(this.apiClient);
            var productsSearch = new ProductsSearch(productsSearchViewModel);

            var result = await DialogHost.Show(productsSearch, "RootDialog");
            if ((bool) result)
            {
                this.AddToBasket(productsSearchViewModel.SelectedProduct);
            }
        }

        public async void HandleF10()
        {
            if (this.Basket.Products.Any())
            {
                var result = await DialogHost.Show(new Confirm("Бракуване?"), "RootDialog");
                if ((bool) result)
                {
#pragma warning disable 4014
                    this.apiClient.WasteProductsAsync(ViewModelLocator.httpClient, this.Basket.Products.Select(x => new IdQuantityPair
#pragma warning restore 4014
                                                                                                                          {
                                                                                                                              Id = x.Id,
                                                                                                                              Quantity = x.Quantity.GetValueOrDefault()
                                                                                                                          }));
                    this.Basket.Clear();
                }
            }
            else
            {
                NotificationsHost.Error("Добавете продукт", "В списъка трябва да има поне 1 продукт.");
            }
        }

        public async void HandleF11()
        {
            if (this.Basket.Products.Any())
            {
                var viewModel = new ChangeCalculatorViewModel(this.Basket.TotalPrice);
                var dialog = new ChangeCalulator(viewModel);
                var result = await DialogHost.Show(dialog, "RootDialog");
                if ((bool) result)
                {
#pragma warning disable 4014
                    this.apiClient.SellProductsAsync(ViewModelLocator.httpClient, this.Basket.Products.Select(x => new IdQuantityPair
#pragma warning restore 4014
                                                                                                                         {
                                                                                                                             Id = x.Id,
                                                                                                                             Quantity = x.Quantity.GetValueOrDefault()
                                                                                                                         }));
                    this.fiscalPrinterHandler.ExportReceipt(this.Basket.Products);
                    this.Basket.Clear();
                }
            }
            else
            {
                NotificationsHost.Error("Добавете продукт", "В списъка трябва да има поне 1 продукт.");
            }
        }

        public async void HandleF12()
        {
            if (this.Basket.Products.Any())
            {
                var viewModel = new ChangeCalculatorViewModel(this.Basket.TotalPrice);
                var dialog = new ChangeCalulator(viewModel);
                var result = await DialogHost.Show(dialog, "RootDialog");
                if ((bool) result)
                {
#pragma warning disable 4014
                    this.apiClient.SellProductsAsync(ViewModelLocator.httpClient, this.Basket.Products.Select(x => new IdQuantityPair
#pragma warning restore 4014
                                                                                                                         {
                                                                                                                             Id = x.Id,
                                                                                                                             Quantity = x.Quantity.GetValueOrDefault()
                                                                                                                         }));
                    this.Basket.Clear();
                }
            }
            else
            {
                NotificationsHost.Error("Добавете продукт", "В списъка трябва да има поне 1 продукт.");
            }
        }

        public async void HandleF5()
        {
            var result = await DialogHost.Show(new Confirm("Излизане?"), "RootDialog");
            if ((bool) result)
            {
                ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowLogin();
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
                NotificationsHost.Error("Забранено", "Продуктът не може да бъде добавен в този момент.");
            }
        }

        private async void Initialize()
        {
            var userInfo = await this.apiClient.GetUserInfo(ViewModelLocator.httpClient);

            this.FullName = userInfo.FirstName + " " + userInfo.LastName;
        }
    }
}