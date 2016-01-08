namespace Eugenie.Clients.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Newtonsoft.Json;

    public class Product : ViewModelBase, IEditableObject
    {
        private string name;
        private decimal? buyingPrice;
        private MeasureType? measure;
        private DateTime? date;
        private string batch;
        private ObservableCollection<Barcode> barcodes;
        private ObservableCollection<ExpirationDate> expirationDates;

        private bool isInEditMode;
        private Product tempProduct;
        private ICollection<Barcode> tempBarcodes;
        private ICollection<ExpirationDate> tempExpirationDates;

        public Product()
        {
            this.Measure = MeasureType.бр;

            this.DeleteBarcodeCommand = new RelayCommand<Barcode>(this.HandleDeleteBarcodeCommand);
            this.DeleteExpirationDateCommand = new RelayCommand<ExpirationDate>(this.HandleDeleteExpirationDateCommand);
            this.AddExpirationDateCommand = new RelayCommand(this.HandleAddExpirationDateCommand);

            this.Barcodes = new List<Barcode>();
            this.ExpirationDates = new List<ExpirationDate>();
        }

        public ICommand DeleteExpirationDateCommand { get; set; }

        public ICommand AddExpirationDateCommand { get; set; }

        public ICommand DeleteBarcodeCommand { get; set; }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.Set(() => this.Name, ref this.name, value);
            }
        }

        public string NewName { get; set; }

        public decimal? BuyingPrice
        {
            get
            {
                return this.buyingPrice;
            }

            set
            {
                this.Set(() => this.BuyingPrice, ref this.buyingPrice, value);
            }
        }

        public decimal? SellingPrice { get; set; }

        public MeasureType? Measure
        {
            get
            {
                return this.measure;
            }

            set
            {
                this.Set(() => this.Measure, ref this.measure, value);
            }
        }

        public decimal? Quantity { get; set; }

        [JsonIgnore]
        public DateTime? Date
        {
            get
            {
                return this.date;
            }
        
            set
            {
                this.date = value;
                this.Set(() => this.Date, ref this.date, value);
            }
        }
        
        [JsonIgnore]
        public string Batch
        {
            get
            {
                return this.batch;
            }
        
            set
            {
                this.batch = value;
                this.Set(() => this.Batch, ref this.batch, value);
            }
        }

        [JsonIgnore]
        public decimal? QuantityToAdd { get; set; }

        public ICollection<Barcode> Barcodes
        {
            get
            {
                return this.barcodes ?? (this.barcodes = new ObservableCollection<Barcode>());
            }

            set
            {
                this.barcodes = this.barcodes ?? new ObservableCollection<Barcode>();

                this.barcodes.Clear();
                foreach (var barcode in value)
                {
                    this.barcodes.Add(barcode);
                }
            }
        }
        
        public ICollection<ExpirationDate> ExpirationDates
        {
            get
            {
                return this.expirationDates ?? (this.expirationDates = new ObservableCollection<ExpirationDate>());
            }

            set
            {
                this.expirationDates = this.expirationDates ?? new ObservableCollection<ExpirationDate>();

                this.expirationDates.Clear();
                foreach (var expirationDate in value)
                {
                    this.expirationDates.Add(expirationDate);
                }
            }
        }

        #region IEditableObject
        public void BeginEdit()
        {
            if (!this.isInEditMode)
            {
                this.tempProduct = this.MemberwiseClone() as Product;
                this.tempBarcodes = new List<Barcode>(this.Barcodes);
                this.tempExpirationDates = new List<ExpirationDate>(this.ExpirationDates);
                this.isInEditMode = true;
            }
        }

        public void EndEdit()
        {
            if (this.isInEditMode)
            {
                this.tempProduct = null;
                this.tempBarcodes = null;
                this.tempExpirationDates = null;
                this.isInEditMode = false;
            }
        }

        //TODO: add all properties
        public void CancelEdit()
        {
            if (this.isInEditMode)
            {
                this.Name = this.tempProduct.Name;
                this.BuyingPrice = this.tempProduct.BuyingPrice;
                this.Measure = this.tempProduct.Measure;
                this.Barcodes = this.tempBarcodes;
                this.isInEditMode = false;
            }
        }
        #endregion

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherProduct = obj as Product;
            return otherProduct != null && this.Name == otherProduct.Name;
        }

        private void HandleDeleteBarcodeCommand(Barcode barcode)
        {
            this.Barcodes.Remove(barcode);
        }

        //TODO: add validation
        private void HandleAddExpirationDateCommand()
        {
            this.ExpirationDates.Add(new ExpirationDate(this.Date.GetValueOrDefault(), this.Batch));
            this.Date = null;
            this.Batch = null;
        }

        private void HandleDeleteExpirationDateCommand(ExpirationDate expirationDate)
        {
            this.ExpirationDates.Remove(expirationDate);
        }
    }
}