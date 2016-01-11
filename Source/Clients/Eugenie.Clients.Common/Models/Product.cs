namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using GalaSoft.MvvmLight;

    using Helpers;

    public class Product : ViewModelBase, IDataErrorInfo
    {
        private string name;
        private decimal buyingPrice;
        private MeasureType measure;
        private ObservableCollection<Barcode> barcodes;
        private ObservableCollection<ExpirationDate> expirationDates;

        public Product()
        {
            this.Measure = MeasureType.бр;

            this.Barcodes = new List<Barcode>();
            this.ExpirationDates = new List<ExpirationDate>();
        }
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.Set(() => this.Name, ref this.name, value.TrimStart());
            }
        }

        public decimal BuyingPrice
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

        public MeasureType Measure
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
        
        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case "Name":
                        return Validator.ValidateProductName(this.Name);
                    default:
                        return null;
                }
            }
        }

        public string Error { get; }
    }
}