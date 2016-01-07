namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;

    using GalaSoft.MvvmLight;

    public class DeliveryViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly IServerStorage storage;
        private readonly IServerManager manager;
        private readonly INameFromBarcodeGenerator nameGenerator;

        private Product mainProduct;
        private string name;
        private string addingType = "Въведете име";

        public DeliveryViewModel(IServerStorage storage, IServerManager manager, INameFromBarcodeGenerator nameGenerator)
        {
            this.storage = storage;
            this.storage.ServerAdded += this.OnServerAdded;
            this.storage.ServerDeleted += this.OnServerDeleted;
            this.nameGenerator = nameGenerator;
            this.manager = manager;
            
            this.AutomaticName = true;

            this.Products = new ObservableDictionary<ServerInformation, Product>();
            foreach (var server in this.storage.Servers)
            {
                this.Products.Add(server, new Product());
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
                    this.MainProduct = new Product();
                }
                else
                {
                    var existingProduct = this.ExistingProducts.FirstOrDefault(x => x.Name == value);
                    if (existingProduct != null)
                    {
                        this.AddingType = "Добавяне на наличност";
                        this.MainProduct = existingProduct;
                    }
                    else
                    {
                        this.AddingType = "Добавяне на нов продукт";
                        this.MainProduct = new Product();
                    }
                }

                this.Set( () => this.Name, ref this.name, value);
            }
        }
        
        public Product MainProduct
        {
            get
            {
                return this.mainProduct ?? (this.mainProduct = new Product());
            }

            set
            {
                this.MainProduct.CancelEdit();
                this.Set(() => this.MainProduct, ref this.mainProduct, value);
                this.MainProduct.BeginEdit();
            }
        }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public IEnumerable<Product> ExistingProducts => this.manager.Cache.Products;

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        public async void HandleBarcode(string barcode)
        {
            var existingProduct = this.ExistingProducts.FirstOrDefault(x => x.Barcodes.Any(y => y.Value == barcode));

            if (existingProduct == null)
            {
                if (this.MainProduct.Barcodes.All(x => x.Value != barcode))
                {
                    if (this.AutomaticName && string.IsNullOrEmpty(this.Name))
                    {
                        this.Name = await this.nameGenerator.GetName(barcode);
                    }

                    this.MainProduct.Barcodes.Add(new Barcode(barcode));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Name = existingProduct.Name;
                    if (this.MainProduct.Barcodes.All(x => x.Value != barcode))
                    {
                        this.MainProduct.Barcodes.Add(new Barcode(barcode));
                    }
                }
                else
                {
                    //TODO: notify barcode exists
                }
            }
        }

        private void OnServerDeleted(object sender, ServerDeletedEventArgs e)
        {
            this.Products.Remove(e.Server);
        }

        private void OnServerAdded(object sender, ServerAddedEventArgs e)
        {
            this.Products.Add(e.Server, new Product());
        }
    }
}