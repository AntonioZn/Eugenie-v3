namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.WebApiModels;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class ProductViewModel : ViewModelBase, IValidatableObject
    {
        private DateTime? date;
        private string batch;
        private string quantityToAdd;
        private string sellingPrice;
        private string buyingPrice;

        public ProductViewModel(Product product)
        {
            this.Product = product;
            this.OldName = product.Name;
            this.SellingPrice = product.SellingPrice.ToString();
            this.BuyingPrice = product.BuyingPrice.ToString();

            this.DeleteBarcodeCommand = new RelayCommand<Barcode>(this.HandleDeleteBarcodeCommand);
            this.DeleteExpirationDateCommand = new RelayCommand<ExpirationDate>(this.HandleDeleteExpirationDateCommand);
            this.AddExpirationDateCommand = new RelayCommand(this.HandleAddExpirationDateCommand, this.CanAddExpirationDate);
        }

        public ICommand DeleteExpirationDateCommand { get; set; }

        public ICommand AddExpirationDateCommand { get; set; }

        public ICommand DeleteBarcodeCommand { get; set; }

        public Product Product { get; set; }

        public string OldName { get; set; }

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
        
        public string BuyingPrice
        {
            get
            {
                return this.buyingPrice;
            }

            set
            {
                this.Set(() => this.BuyingPrice, ref this.buyingPrice, value?.Trim());
            }
        }

        public string QuantityToAdd
        {
            get
            {
                return this.quantityToAdd;
            }

            set
            {
                this.Set(() => this.QuantityToAdd, ref this.quantityToAdd, value?.Trim());
            }
        }

        public string SellingPrice
        {
            get
            {
                return this.sellingPrice;
            }

            set
            {
                this.Set(() => this.SellingPrice, ref this.sellingPrice, value?.Trim());
            }
        }

        public void MapProperties(ProductViewModel baseProduct)
        {
            this.Product.Name = baseProduct.Product.Name.Trim();
            this.Product.BuyingPrice = decimal.Parse(baseProduct.BuyingPrice);
            this.Product.SellingPrice = string.IsNullOrEmpty(this.SellingPrice) ? this.Product.SellingPrice : decimal.Parse(this.SellingPrice);
            this.Product.Measure = baseProduct.Product.Measure;
            this.Product.Barcodes = baseProduct.Product.Barcodes;

            this.Product.Quantity = this.Product.Quantity ?? 0;
            this.Product.Quantity += string.IsNullOrEmpty(this.QuantityToAdd) ? 0 : decimal.Parse(this.QuantityToAdd);
        }

        public AddProductModel GetModel()
        {
            var model = new AddProductModel
                        {
                            Name = this.Product.Name,
                            OldName = this.OldName,
                            Measure = this.Product.Measure,
                            ExpirationDates = this.Product.ExpirationDates,
                            Barcodes = this.Product.Barcodes
                        };

            model.SellingPrice = this.Product.SellingPrice;
            model.BuyingPrice = this.Product.BuyingPrice;

            if (!string.IsNullOrEmpty(this.QuantityToAdd))
            {
                model.QuantityToAdd = decimal.Parse(this.QuantityToAdd);
            }

            this.QuantityToAdd = null;

            return model;
        }

        private void HandleDeleteBarcodeCommand(Barcode barcode)
        {
            this.Product.Barcodes.Remove(barcode);
        }

        private void HandleAddExpirationDateCommand()
        {
            this.Product.ExpirationDates.Add(new ExpirationDate(this.Date.GetValueOrDefault(), this.Batch));
            this.Date = null;
            this.Batch = null;
        }

        private bool CanAddExpirationDate()
        {
            return this.Date != null
                && this.Product.ExpirationDates.All(x => x.Date != this.Date || (x.Batch ?? "") != (this.Batch ?? ""));
        }

        private void HandleDeleteExpirationDateCommand(ExpirationDate expirationDate)
        {
            this.Product.ExpirationDates.Remove(expirationDate);
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(this.QuantityToAdd):
                        return Validator.ValidateNullableDecimal(this.QuantityToAdd);
                    case nameof(this.SellingPrice):
                        return Validator.ValidateNullableDecimal(this.SellingPrice);
                    case nameof(this.BuyingPrice):
                        return Validator.ValidateNotNullableDecimal(this.BuyingPrice);
                    default:
                        return null;
                }
            }
        }

        public string Error { get; }

        public bool HasNoValidationErrors()
        {
            return this[nameof(this.QuantityToAdd)] == null
                && this[nameof(this.SellingPrice)] == null
                && this[nameof(this.BuyingPrice)] == null;
        }
    }
}