namespace Eugenie.Clients.Seller.ViewModels
{
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
            var viewModel = new ProductsSearchViewModel(this.apiClient);
            var dialog = new ProductsSearch(viewModel);

            var result = await DialogHost.Show(dialog, "RootDialog");
            if ((bool)result)
            {
                var product = viewModel.SelectedProduct;
                product.Quantity = 1;
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