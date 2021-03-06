﻿namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.Notifications;
    using Common.Еxtensions;
    
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using Models;

    using Store = Models.Store;

    public class DeliveryViewModel : ViewModelBase, IValidatableObject, IBarcodeHandler
    {
        private readonly ServerManager manager;
        private readonly ObservableCollection<Product> products = new ObservableCollection<Product>();
        private readonly DispatcherTimer timer;

        private ProductViewModel mainMainProductViewModel;
        private IDictionary<Store, ProductViewModel> productInAllServers;
        private string name = string.Empty;
        private string addingType = "Въведете име";
        private Product selectedProduct;
        private bool lastProductIsExisting;

        public DeliveryViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
            this.manager.ProductsCacheChanged += this.OnServerTestingFinished;

            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(0.5);
            this.timer.Tick += this.OnTick;

            this.Save = new RelayCommand(this.HandleSave, this.CanSave);
            this.Cancel = new RelayCommand(this.HandleCancel);
        }

        public ICommand Save { get; }

        public ICommand Cancel { get; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        public IDictionary<Store, ProductViewModel> ProductInAllServers
        {
            get => this.productInAllServers ?? (this.productInAllServers = new ObservableConcurrentDictionary<Store, ProductViewModel>());
            set
            {
                this.productInAllServers = this.productInAllServers ?? new ObservableConcurrentDictionary<Store, ProductViewModel>();
                this.productInAllServers.Clear();
                foreach (var pair in value)
                {
                    this.productInAllServers.Add(pair.Key, pair.Value);
                }
            }
        }

        public ICollectionView Products => CollectionViewSource.GetDefaultView(this.products);

        public bool AutomaticName { get; set; } = true;

        public string AddingType
        {
            get => this.addingType;
            set
            {
                this.Set(() => this.AddingType, ref this.addingType, value);
            }
        }

        public ProductViewModel MainProductViewModel
        {
            get => this.mainMainProductViewModel;
            set
            {
                this.Set(() => this.MainProductViewModel, ref this.mainMainProductViewModel, value);
            }
        }

        public Product SelectedProduct
        {
            get => this.selectedProduct;
            set
            {
                this.Set(() => this.SelectedProduct, ref this.selectedProduct, value);
                if (value != null)
                {
                    this.Set(() => this.Name, ref this.name, value.Name);
                    this.lastProductIsExisting = true;
                    this.GetExistingProduct(value);
                    this.AddingType = "Добавяне на наличност";
                }
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                this.Set(() => this.Name, ref this.name, value.RemoveMultipleWhiteSpaces().ToLower());
                if (this.Name != null)
                {
                    this.timer.Stop();
                    this.timer.Start();
                }
            }
        }

        public string Error { get; }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(this.Name):
                        return Validator.ValidateProductName(this.Name);
                    default:
                        return null;
                }
            }
        }

        public void HandleSearch(string name)
        {
            name = name.ToLower();
            if (string.IsNullOrWhiteSpace(name))
            {
                this.AddingType = "Въведете име";
                this.GetNewProduct(string.Empty);
                this.RemoveFilter();
            }
            else
            {
                this.FilterProducts();

                var existingProduct = this.products.FirstOrDefault(x => x.Name == name);
                if (existingProduct != null)
                {
                    this.AddingType = "Добавяне на наличност";
                    this.lastProductIsExisting = true;
                    this.GetExistingProduct(existingProduct);
                }
                else
                {
                    this.AddingType = "Добавяне на нов продукт";
                    this.MainProductViewModel.Product.Name = name;

                    if (this.lastProductIsExisting)
                    {
                        this.lastProductIsExisting = false;
                        this.GetNewProduct(name);
                    }
                }
            }
        }

        public async void HandleBarcode(string barcode)
        {
            var existingProduct = ExistingBarcodeChecker.Check(barcode, this.MainProductViewModel.Product, this.products);
            if (existingProduct != null)
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.SelectedProduct = existingProduct;
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
                    var name = await NameFromBarcodeGenerator.GetName(barcode);
                    this.Set(() => this.Name, ref this.name, name);
                }

                this.MainProductViewModel.Product.Barcodes.Add(new Barcode(barcode));
            }
        }

        public void ImportMissingProduct(string name, string barcode)
        {
            name = name.ToLower();
            this.Set(() => this.Name, ref this.name, name);
            this.GetNewProduct(string.Empty);
            this.HandleSearch(name);
            this.HandleBarcode(barcode);
        }

        public bool HasNoValidationErrors()
        {
            return this[nameof(this.Name)] == null;
        }

        private void OnTick(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.HandleSearch(this.Name);
        }

        private void HandleSave()
        {
            this.manager.AddOrUpdate(this.ProductInAllServers, this.MainProductViewModel);
            this.Reset();
        }

        private bool CanSave()
        {
            return this.HasNoValidationErrors()
                   && this.MainProductViewModel.Product.HasNoValidationErrors()
                   && this.MainProductViewModel.HasNoValidationErrors()
                   && this.ProductInAllServers.Values.All(x => x.HasNoValidationErrors());
        }

        private void HandleCancel()
        {
            if (!string.IsNullOrWhiteSpace(this.Name))
            {
                this.Reset();
            }
            else
            {
                ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowProductsEditor();
            }
        }

        private void Reset()
        {
            this.Set(() => this.Name, ref this.name, null);
            this.AddingType = "Въведете име";
            this.GetNewProduct(string.Empty);
            this.RemoveFilter();
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.products.Clear();
            this.manager.Stores.OrderByDescending(x => x.Products.Count).FirstOrDefault()?.Products.ForEach(this.products.Add);
            this.GetNewProduct(string.Empty);
        }

        private void FilterProducts()
        {
            var searchAsArray = this.Name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            this.Products.Filter = obj =>
            {
                var product = obj as Product;

                return searchAsArray.All(word => product.Name.Contains(word));
            };
        }

        private void RemoveFilter()
        {
            this.Products.Filter = null;
            this.Products.Refresh();
        }

        private void GetNewProduct(string name)
        {
            this.MainProductViewModel = new ProductViewModel(new Product { Name = name });

            var tempProductInAllServers = new Dictionary<Store, ProductViewModel>();
            foreach (var store in this.manager.Stores)
            {
                var productViewModel = new ProductViewModel(new Product());
                tempProductInAllServers.Add(store, productViewModel);
            }

            this.ProductInAllServers = tempProductInAllServers;
        }

        private void GetExistingProduct(Product existingProduct)
        {
            this.MainProductViewModel = new ProductViewModel(existingProduct.DeepClone());

            var tempProductInAllServers = new Dictionary<Store, ProductViewModel>();
            foreach (var store in this.manager.Stores)
            {
                var product = store.Products.FirstOrDefault(x => x.Name == this.Name);
                var productViewModel = new ProductViewModel(product ?? new Product());
                tempProductInAllServers.Add(store, productViewModel);
            }

            this.ProductInAllServers = tempProductInAllServers;
        }
    }
}