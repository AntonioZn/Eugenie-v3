namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Helpers;

    using Sv.Wpf.Core.Extensions;
    using Sv.Wpf.Core.Mvvm;

    public class Product : ViewModelBase, IValidatableObject
    {
        private ObservableCollection<Barcode> barcodes;
        private decimal buyingPrice;
        private ObservableCollection<ExpirationDate> expirationDates;
        private MeasureType measure;
        private string name;
        private decimal? quantity;
        private decimal? sellingPrice;

        public Product()
        {
            this.Measure = MeasureType.бр;

            this.Barcodes = new List<Barcode>();
            this.ExpirationDates = new List<ExpirationDate>();
        }

        public int Id { get; set; }

        public string Name
        {
            get => this.name;
            set => this.Set(ref this.name, value.TrimMultipleWhiteSpaces());
        }

        public decimal BuyingPrice
        {
            get => this.buyingPrice;
            set => this.Set(ref this.buyingPrice, value);
        }

        public decimal? SellingPrice
        {
            get => this.sellingPrice;
            set => this.Set(ref this.sellingPrice, value);
        }

        public MeasureType Measure
        {
            get => this.measure;
            set => this.Set(ref this.measure, value);
        }

        public decimal? Quantity
        {
            get => this.quantity;
            set => this.Set(ref this.quantity, value);
        }

        public ICollection<Barcode> Barcodes
        {
            get => this.barcodes ?? (this.barcodes = new ObservableCollection<Barcode>());
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
            get => this.expirationDates ?? (this.expirationDates = new ObservableCollection<ExpirationDate>());
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

        public new string this[string propertyName]
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
    }
}