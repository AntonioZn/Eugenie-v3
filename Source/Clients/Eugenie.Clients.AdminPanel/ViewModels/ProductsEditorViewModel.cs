namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Common.Contracts;
    using Common.Messages;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ProductsEditorViewModel : ViewModelBase
    {
        private readonly IWebApiServerClient client;
        private Visibility loadingVisibility;
        private ObservableCollection<SimplifiedProduct> products;
        private SimplifiedProduct selectedItem;

        public ProductsEditorViewModel(IWebApiServerClient client)
        {
            this.client = client;
            this.LoadingVisibility = Visibility.Collapsed;
            Messenger.Default.Register<ServerTestingFinishedMessage>(this, this.OnServerTestingFinishedMessage);
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

        public IEnumerable<SimplifiedProduct> Products
        {
            get
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<SimplifiedProduct>();
                }

                return this.products;
            }

            set
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<SimplifiedProduct>();
                }

                this.products.Clear();
                foreach (var product in value)
                {
                    this.products.Add(product);
                }
            }
        }

        public SimplifiedProduct SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                this.Set(() => this.SelectedItem, ref this.selectedItem, value);
            }
        }

        public async void ShowProductInformationDialog()
        {
            var productInAllServers = await this.client.GetProductById(this.selectedItem.Id);
            var model = new ProductInformationViewModel(productInAllServers);
            var dialog = new ProductInformation(model);
            this.DialogIsOpen = true;
            var result = await DialogHost.Show(dialog, "RootDialog");
            this.DialogIsOpen = false;
        }

        private async void OnServerTestingFinishedMessage(ServerTestingFinishedMessage obj)
        {
            this.LoadingVisibility = Visibility.Visible;
            this.Products = await this.client.GetProductsByPageAsync(1, 2000);
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}