namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Windows.Input;

    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class ProductViewModel : ViewModelBase
    {
        private DateTime? date;
        private string batch;

        public ProductViewModel(Product product)
        {
            this.Product = product;
            this.OldName = product.Name;

            this.DeleteBarcodeCommand = new RelayCommand<Barcode>(this.HandleDeleteBarcodeCommand);
            this.DeleteExpirationDateCommand = new RelayCommand<ExpirationDate>(this.HandleDeleteExpirationDateCommand);
            this.AddExpirationDateCommand = new RelayCommand(this.HandleAddExpirationDateCommand, this.CanAddExpirationDate);
        }

        public Product Product { get; set; }

        public ICommand DeleteExpirationDateCommand { get; set; }

        public ICommand AddExpirationDateCommand { get; set; }

        public ICommand DeleteBarcodeCommand { get; set; }

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

        public decimal? QuantityToAdd { get; set; }

        public void MapProperties(ProductViewModel baseProduct)
        {
            this.Product.Name = baseProduct.Product.Name;
            this.OldName = baseProduct.OldName;
            this.Product.Measure = baseProduct.Product.Measure;
            this.Product.BuyingPrice = baseProduct.Product.BuyingPrice;
            this.Product.Barcodes = baseProduct.Product.Barcodes;
            this.Product.Quantity += this.QuantityToAdd ?? 0;
        }

        public AddProductModel GetModel()
        {
            var model = new AddProductModel();

            model.Name = this.Product.Name;
            model.OldName = this.OldName;
            model.SellingPrice = this.Product.SellingPrice;
            model.BuyingPrice = this.Product.BuyingPrice;
            model.Measure = this.Product.Measure;
            model.QuantityToAdd = this.QuantityToAdd ?? 0;
            model.ExpirationDates = this.Product.ExpirationDates;
            model.Barcodes = this.Product.Barcodes;

            this.QuantityToAdd = null;

            return model;
        }

        private void HandleDeleteBarcodeCommand(Barcode barcode)
        {
            this.Product.Barcodes.Remove(barcode);
        }

        //TODO: add validation
        private void HandleAddExpirationDateCommand()
        {
            this.Product.ExpirationDates.Add(new ExpirationDate(this.Date.GetValueOrDefault(), this.Batch));
            this.Date = null;
            this.Batch = null;
        }

        private bool CanAddExpirationDate()
        {
            return this.Date != null && this.Batch != null;
        }

        private void HandleDeleteExpirationDateCommand(ExpirationDate expirationDate)
        {
            this.Product.ExpirationDates.Remove(expirationDate);
        }
    }
}