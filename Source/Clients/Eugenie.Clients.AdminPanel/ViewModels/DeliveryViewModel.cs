namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class DeliveryViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerStorage storage;
        private readonly IServerManager manager;
        private readonly INameFromBarcodeGenerator nameGenerator;
        private readonly IProductsCache cache;
        private ObservableCollection<SimplifiedProduct> exisitingProducts;
        private string name;
        private string addingType;
        private DateTime? date;
        private string batch;
        private SimplifiedProduct firstProduct;

        public DeliveryViewModel(IServerStorage storage, IServerManager manager, INameFromBarcodeGenerator nameGenerator, IProductsCache cache)
        {
            this.storage = storage;
            this.nameGenerator = nameGenerator;
            this.cache = cache;
            this.manager = manager;
            manager.ServerTestingFinished += this.OnServerTestingFinished;
            
            this.AutomaticName = true;
            this.AddingType = "Въведете име";

            this.Measures = new List<MeasureType>();
            foreach (var measureType in Enum.GetValues(typeof(MeasureType)))
            {
                this.Measures.Add((MeasureType)measureType);
            }

            this.Products = new Dictionary<ServerInformation, Product>();
            foreach (var server in this.storage.Servers)
            {
                this.Products.Add(server, new Product());
            }

            this.DeleteBarcodeCommand = new RelayCommand<Barcode>(this.HandleDeleteBarcodeCommand);
            this.DeleteExpirationDateCommand = new RelayCommand<object>(this.HandleDeleteExpirationDateCommand);
            this.AddExpirationDateCommand = new RelayCommand<Product>(this.HandleAddExpirationDateCommand);
        }
        
        public ICommand DeleteExpirationDateCommand { get; set; }

        public ICommand AddExpirationDateCommand { get; set; }

        public ICommand DeleteBarcodeCommand { get; set; }

        public string Barcode
        { 
            set
            {
                this.HandleBarcode(value);
            }
        }

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

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                value = value.Trim();

                if (value == string.Empty)
                {
                    this.AddingType = "Въведете име";
                    this.FirstProduct = new SimplifiedProduct(value, 0, MeasureType.бр, new List<Barcode>());
                }
                else
                {
                    var existingProduct = this.ExistingProducts.FirstOrDefault(x => x.Name == value);
                    if (existingProduct != null)
                    {
                        this.AddingType = "Добавяне на наличност";
                        this.FirstProduct = existingProduct;
                    }
                    else
                    {
                        this.AddingType = "Добавяне на нов продукт";
                        this.FirstProduct = new SimplifiedProduct(value, 0, MeasureType.бр, new List<Barcode>());
                    }
                }

                this.Set( () => this.Name, ref this.name, value);
            }
        }
        
        public SimplifiedProduct FirstProduct
        {
            get
            {
                if (this.firstProduct == null)
                {
                    this.firstProduct = new SimplifiedProduct(string.Empty, 0, MeasureType.бр, new List<Barcode>());
                }

                return this.firstProduct;
            }

            set
            {
                this.FirstProduct.CancelEdit();
                this.Set(() => this.FirstProduct, ref this.firstProduct, value);
                this.FirstProduct.BeginEdit();
            }
        }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public IEnumerable<SimplifiedProduct> ExistingProducts
        {
            get
            {
                if (this.exisitingProducts == null)
                {
                    this.exisitingProducts = new ObservableCollection<SimplifiedProduct>();
                }

                return this.exisitingProducts;
            }

            set
            {
                if (this.exisitingProducts == null)
                {
                    this.exisitingProducts = new ObservableCollection<SimplifiedProduct>();
                }

                this.exisitingProducts.Clear();
                foreach (var simplifiedProduct in value)
                {
                    this.exisitingProducts.Add(simplifiedProduct);
                }
            }
        }

        public ICollection<MeasureType> Measures { get; set; }

        public DateTime? Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.Set(() => this.Date, ref this.date, value);
            }
        }

        public string Batch
        {
            get
            {
                return this.batch;
            }

            set
            {
                this.Set(() => this.Batch, ref this.batch, value);
            }
        }

        private async void HandleBarcode(string barcode)
        {
            var existingProduct = this.ExistingProducts.FirstOrDefault(x => x.Barcodes.Any(y => y.Value == barcode));

            if (existingProduct == null)
            {
                if (this.FirstProduct.Barcodes.All(x => x.Value != barcode))
                {
                    if (this.AutomaticName && string.IsNullOrEmpty(this.Name))
                    {
                        this.Name = await this.nameGenerator.GetName(barcode);
                    }

                    this.FirstProduct.Barcodes.Add(new Barcode(barcode));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = existingProduct.Name;
                    if (this.FirstProduct.Barcodes.All(x => x.Value != barcode))
                    {
                        this.FirstProduct.Barcodes.Add(new Barcode(barcode));
                    }
                }
                else
                {
                    //TODO: notify barcode exists
                }
            }
        }

        private void HandleDeleteBarcodeCommand(Barcode barcode)
        {
            this.FirstProduct.Barcodes.Remove(barcode);
        }

        //TODO: add validation
        private void HandleAddExpirationDateCommand(Product product)
        {
            product.ExpirationDates.Add(new ExpirationDate(this.Date.GetValueOrDefault(), this.Batch));
            this.Date = null;
            this.Batch = null;
        }

        private void HandleDeleteExpirationDateCommand(object parameter)
        {
            var values = (object[])parameter;
            var expirationDate = values[0] as ExpirationDate;
            var product = values[1] as Product;
            product.ExpirationDates.Remove(expirationDate);
        }

        private async void OnServerTestingFinished(object sender, EventArgs e)
        {
            if (this.manager.ActiveServers.Any())
            {
                this.ExistingProducts = await this.manager.GetProductsByPageAsync(1, int.MaxValue);
            }
        }
    }
}
