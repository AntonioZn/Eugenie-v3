namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;
    using Common.Models;

    using Data.Models;

    using GalaSoft.MvvmLight;

    using Product = Common.Models.Product;

    public class AddNewProductViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerStorage storage;
        private readonly IServerManager manager;
        private ObservableCollection<string> exisitingProducts;
        private string name;
        private string addingType;
        private string barcode;

        private AddNewProductViewModel()
        {
            this.AutomaticName = true;
            this.AddingType = "Въведете име";
            this.Measures = new List<MeasureType>();
            foreach (var measureType in Enum.GetValues(typeof(MeasureType)))
            {
                this.Measures.Add((MeasureType)measureType);
            }

            this.Products = new Dictionary<ServerInformation, Product>();
        }

        public AddNewProductViewModel(IServerStorage storage, IServerManager manager) : this()
        {
            this.storage = storage;
            this.manager = manager;
            manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.FirstProduct = new SimplifiedProduct("сол", 10, MeasureType.бр, new List<Barcode> { new Barcode { Value = "123123123" } });
            
            foreach (var server in this.storage.Servers)
            {
                this.Products.Add(server, new Product());
            }
        }

        public AddNewProductViewModel(IServerStorage storage, IServerManager manager, SimplifiedProduct simplifiedProduct) : this()
        {
            this.storage = storage;
            this.manager = manager;
            manager.ServerTestingFinished += this.OnServerTestingFinished;
            
            this.FirstProduct = simplifiedProduct;

            foreach (var server in this.storage.Servers)
            {
                this.Products.Add(server, new Product());
            }
        }

        public string Barcode
        {
            get
            {
                return this.barcode;
            }

            set
            {
                this.barcode = value;
            }
        }

        public bool AutomaticName { get; set; }

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
                }
                else if (this.ExistingProductsNames.Contains(value))
                {
                    this.AddingType = "Добавяне на наличност";
                }
                else
                {
                    this.AddingType = "Добавяне на нов продукт";
                }

                this.Set( () => this.Name, ref this.name, value);
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

        public SimplifiedProduct FirstProduct { get; set; }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public IEnumerable<string> ExistingProductsNames
        {
            get
            {
                if (this.exisitingProducts == null)
                {
                    this.exisitingProducts = new ObservableCollection<string>();
                }

                return this.exisitingProducts;
            }

            set
            {
                if (this.exisitingProducts == null)
                {
                    this.exisitingProducts = new ObservableCollection<string>();
                }

                this.exisitingProducts.Clear();
                foreach (var simplifiedProduct in value)
                {
                    this.exisitingProducts.Add(simplifiedProduct);
                }
            }
        }

        public ICollection<MeasureType> Measures { get; set; }

        private async void OnServerTestingFinished(object sender, EventArgs e)
        {
            var products = await this.manager.GetProductsByPageAsync(1, int.MaxValue);
            this.ExistingProductsNames = products.Select(x => x.Name);
        }
    }
}
