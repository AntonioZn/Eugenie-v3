namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class DeliveryViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerManager manager;
        private readonly INameFromBarcodeGenerator nameGenerator;
        
        private IDictionary<ServerInformation, ProductViewModel> productInAllServers;
        private ProductViewModel mainMainProductViewModel;
        private string name = string.Empty;
        private string addingType = "Въведете име";

        public DeliveryViewModel(IServerManager manager, INameFromBarcodeGenerator nameGenerator)
        {
            this.nameGenerator = nameGenerator;
            this.manager = manager;

            this.AutomaticName = true;

            this.AddCommand = new RelayCommand<UserControl>(this.HandleAddCommand, this.CanAddCommand);
            this.CancelCommand = new RelayCommand(this.HandleCancelCommand);
        }

        public ICommand AddCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public IDictionary<ServerInformation, ProductViewModel> ProductInAllServers
        {
            get
            {
                return this.productInAllServers ?? (this.productInAllServers = new ObservableConcurrentDictionary<ServerInformation, ProductViewModel>());
            }

            set
            {
                this.productInAllServers = this.productInAllServers ?? (this.productInAllServers = new ObservableConcurrentDictionary<ServerInformation, ProductViewModel>());
                this.productInAllServers.Clear();
                foreach (var pair in value)
                {
                    this.productInAllServers.Add(pair.Key, pair.Value);
                }
            }
        }

        public IEnumerable<Product> ExistingProducts => this.manager.Cache.Products;

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        public bool AutomaticName { get; set; }

        public string AddingType
        {
            get
            {
                return this.addingType;
            }

            set
            {
                this.Set(() => this.AddingType, ref this.addingType, value);
            }
        }

        public ProductViewModel MainProductViewModel
        {
            get
            {
                return this.mainMainProductViewModel;
            }

            set
            {
                this.Set(() => this.MainProductViewModel, ref this.mainMainProductViewModel, value);
            }
        }
        
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                value = value.Trim();

                var existingProduct = this.ExistingProducts.FirstOrDefault(x => x.Name == value);
                if (existingProduct != null)
                {
                    this.AddingType = "Добавяне на наличност";

                    this.MainProductViewModel = new ProductViewModel(existingProduct.DeepClone());

                    var tempProductInAllServers = new Dictionary<ServerInformation, ProductViewModel>();
                    foreach (var pair in this.manager.Cache.ProductsPerServer)
                    {
                        var product = pair.Value.FirstOrDefault(x => x.Name == value);
                        var productViewModel = new ProductViewModel(product ?? new Product());
                        tempProductInAllServers.Add(pair.Key, productViewModel);
                    }

                    this.ProductInAllServers = tempProductInAllServers;
                }
                else
                {
                    this.AddingType = string.IsNullOrEmpty(value) ? "Въведете име" : "Добавяне на нов продукт";

                    this.MainProductViewModel = new ProductViewModel(new Product { Name = value });

                    var tempProductInAllServers = new Dictionary<ServerInformation, ProductViewModel>();
                    foreach (var pair in this.manager.Cache.ProductsPerServer)
                    {
                        var productViewModel = new ProductViewModel(new Product());
                        tempProductInAllServers.Add(pair.Key, productViewModel);
                    }

                    this.ProductInAllServers = tempProductInAllServers;
                }

                this.Set(() => this.Name, ref this.name, value);
            }
        }

        public async void HandleBarcode(string barcode)
        {
            var existingProduct = this.ExistingProducts.FirstOrDefault(x => x.Barcodes.Any(y => y.Value == barcode));

            if (existingProduct == null)
            {
                if (this.MainProductViewModel.Product.Barcodes.All(x => x.Value != barcode))
                {
                    if (this.AutomaticName && string.IsNullOrEmpty(this.Name))
                    {
                        this.Name = await this.nameGenerator.GetName(barcode);
                    }

                    this.MainProductViewModel.Product.Barcodes.Add(new Barcode(barcode));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = existingProduct.Name;
                    if (this.MainProductViewModel.Product.Barcodes.All(x => x.Value != barcode))
                    {
                        this.MainProductViewModel.Product.Barcodes.Add(new Barcode(barcode));
                    }
                }
                else
                {
                    //TODO: notify barcode exists
                }
            }
        }

        //TODO: when new item is added add it to cache
        private async void HandleAddCommand(UserControl arg)
        {
            foreach (var pair in this.ProductInAllServers)
            {
                pair.Value.MapProperties(this.MainProductViewModel);
                await this.manager.AddOrUpdateAsync(pair.Key, pair.Value.GetModel());
            }

            this.Name = string.Empty;
        }

        private bool CanAddCommand(UserControl arg)
        {
            return !string.IsNullOrEmpty(this.Name) && arg.HasNoValidationErrors();
        }

        private void HandleCancelCommand()
        {
            this.Name = string.Empty;
        }
    }
}