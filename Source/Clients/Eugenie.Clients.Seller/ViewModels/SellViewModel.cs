namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Contracts.KeyHandlers;
    using Common.Models;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class SellViewModel : ViewModelBase, IBarcodeHandler, IDeleteHandler, IEnterHandler, IF1Handler
    {
        private readonly IWebApiClient apiClient;
        private string fullname;

        public SellViewModel(IWebApiClient apiClient)
        {
            this.apiClient = apiClient;

            this.Basket = new BasketViewModel();

            this.Initialize();
        }

        public ICommand Enter { get; }

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

        public void HandleDelete()
        {
            this.Basket.Delete(this.SelectedProduct);
        }

        public void HandleEnter()
        {
            if (this.SelectedProduct != null)
            {
                this.AddToBasket(this.SelectedProduct);
            }
        }

        public async void HandleF1()
        {
            var productsSearchViewModel = new ProductsSearchViewModel(this.apiClient);
            var productsSearch = new ProductsSearch(productsSearchViewModel);

            var result = await DialogHost.Show(productsSearch, "RootDialog");
            if ((bool)result)
            {
                this.AddToBasket(productsSearchViewModel.SelectedProduct);
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

            var quantityEditorViewModel = new QuantityEditorViewModel(startingQuantity, product.Measure);
            var quantityEditor = new QuantityEditor(quantityEditorViewModel);
            var result = await DialogHost.Show(quantityEditor, "RootDialog");

            if ((bool)result)
            {
                product.Quantity = decimal.Parse(quantityEditorViewModel.Quantity);
                this.Basket.Add(product);
            }
        }

        private async void Initialize()
        {
            var userInfo = await this.apiClient.GetUserInfo(ViewModelLocator.httpClient);

            this.FullName = userInfo.FirstName + " " + userInfo.LastName;
        }
    }
}