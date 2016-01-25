namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MaterialDesignThemes.Wpf;

    using Notifications;

    public class DeliveryViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerManager manager;

        private ObservableCollection<Product> existingProducts;
        private IDictionary<ServerInformation, ProductViewModel> productInAllServers;
        private ProductViewModel mainMainProductViewModel;
        private string name = string.Empty;
        private string addingType = "Въведете име";
        private bool automaticName = true;

        public DeliveryViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
            this.manager.ProductsCacheChanged += this.OnServerTestingFinished;

            this.MainProductViewModel = new ProductViewModel(new Product());

            this.SaveCommand = new RelayCommand(this.HandleSaveCommand, this.CanSave);
            this.CancelCommand = new RelayCommand(this.HandleCancelCommand);
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        public IDictionary<ServerInformation, ProductViewModel> ProductInAllServers
        {
            get
            {
                return this.productInAllServers ?? (this.productInAllServers = new ObservableConcurrentDictionary<ServerInformation, ProductViewModel>());
            }

            set
            {
                this.productInAllServers = this.productInAllServers ?? new ObservableConcurrentDictionary<ServerInformation, ProductViewModel>();
                this.productInAllServers.Clear();
                foreach (var pair in value)
                {
                    this.productInAllServers.Add(pair.Key, pair.Value);
                }
            }
        }

        public IEnumerable<Product> ExistingProducts
        {
            get
            {
                return this.existingProducts ?? (this.existingProducts = new ObservableCollection<Product>());
            }

            set
            {
                this.existingProducts = this.existingProducts ?? new ObservableCollection<Product>();
                this.existingProducts.Clear();
                value.ForEach(this.existingProducts.Add);
            }
        }

        public bool AutomaticName
        {
            get
            {
                return this.automaticName;
            }

            set
            {
                this.Set(() => this.AutomaticName, ref this.automaticName, value);
            }
        }

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
            var existingProduct = ExistingBarcodeChecker.Check(barcode, this.MainProductViewModel.Product, this.ExistingProducts);
            if (existingProduct != null)
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = existingProduct.Name;
                }
                else
                {
                    NotificationsHost.Error("Баркодът съществува", $"\"{existingProduct.Name}\" съдържа този баркод.");
                }
            }
            else
            {
                if (this.AutomaticName && string.IsNullOrEmpty(this.Name))
                {
                    this.Name = await NameFromBarcodeGenerator.GetName(barcode);
                }

                this.MainProductViewModel.Product.Barcodes.Add(new Barcode(barcode));
            }
        }

        private void HandleSaveCommand()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
            foreach (var pair in this.ProductInAllServers)
            {
                pair.Value.MapProperties(this.MainProductViewModel);
                this.manager.AddOrUpdate(pair.Key, pair.Value.GetModel());

                if (!this.AddingType.Contains("наличност"))
                {
                    this.manager.AddProductToCache(pair.Value.Product);
                }
            }

            this.Name = string.Empty;
        }

        private bool CanSave()
        {
            return this.MainProductViewModel.Product.HasNoValidationErrors()
                && this.MainProductViewModel.HasNoValidationErrors()
                && this.ProductInAllServers.Values.All(x => x.HasNoValidationErrors());
        }

        private void HandleCancelCommand()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
            this.Name = string.Empty;
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.ExistingProducts = this.manager.Cache.ProductsPerServer.FirstOrDefault(x => x.Value.Any()).Value;
        }
    }
}