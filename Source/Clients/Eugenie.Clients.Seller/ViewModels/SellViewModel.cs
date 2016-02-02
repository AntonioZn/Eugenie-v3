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

    public class SellViewModel : ViewModelBase, IBarcodeHandler, IDeleteHandler, IEnterHandler, IEscapeHandler, IF1Handler
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

        public void HandleBarcode(string barcode)
        {
            throw new System.NotImplementedException();
        }

        public void HandleDelete()
        {
            this.Basket.Delete(this.SelectedProduct);
        }

        public void HandleEnter()
        {
            throw new System.NotImplementedException();
        }

        public void HandleEscape()
        {
            throw new System.NotImplementedException();
        }

        public async void HandleF1()
        {
            var productsSearchViewModel = new ProductsSearchViewModel(this.apiClient);
            var productsSearch = new ProductsSearch(productsSearchViewModel);

            var result = await DialogHost.Show(productsSearch, "RootDialog");
            if ((bool)result)
            {
                decimal quantity = 1;
                var existingProduct = this.Basket.Products.FirstOrDefault(x => x.Id == productsSearchViewModel.SelectedProduct.Id);
                if (existingProduct != null)
                {
                    quantity = existingProduct.Quantity.Value + 1;
                }
                
                var quantityEditorViewModel = new QuantityEditorViewModel(quantity, productsSearchViewModel.SelectedProduct.Measure);
                var quantityEditor = new QuantityEditor(quantityEditorViewModel);
                result = await DialogHost.Show(quantityEditor, "RootDialog");

                if ((bool)result)
                {
                    var product = productsSearchViewModel.SelectedProduct;
                    product.Quantity = decimal.Parse(quantityEditorViewModel.Quantity);
                    this.Basket.Add(product);
                }
            }
        }

        private async void Initialize()
        {
            var userInfo = await this.apiClient.GetUserInfo(ViewModelLocator.httpClient);

            this.FullName = userInfo.FirstName + " " + userInfo.LastName;
        }
    }
}