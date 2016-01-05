namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Annotations;

    using Data.Models;

    public class SimplifiedProduct : INotifyPropertyChanged, IEditableObject
    {
        private string name;
        private decimal buyingPrice;
        private MeasureType measure;
        private SimplifiedProduct tempProduct;
        private bool isInEditMode;
        private ObservableCollection<Barcode> barcodes; 
        private IList<Barcode> tempBarcodes;

        public SimplifiedProduct(string name, decimal buyingPrice, MeasureType measure, ICollection<Barcode> barcodes)
        {
            this.Name = name;
            this.BuyingPrice = buyingPrice;
            this.Measure = measure;
            this.Barcodes = barcodes;
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.OnPropertyChanged();
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
                this.buyingPrice = value;
                this.OnPropertyChanged();
            }
        }

        public MeasureType Measure
        {
            get
            {
                return this.measure;
            }

            set
            {
                this.measure = value;
                this.OnPropertyChanged();
            }
        }

        public ICollection<Barcode> Barcodes
        {
            get
            {
                if (this.barcodes == null)
                {
                    this.barcodes = new ObservableCollection<Barcode>();
                }

                return this.barcodes;
            }

            set
            {
                if (this.barcodes == null)
                {
                    this.barcodes = new ObservableCollection<Barcode>();
                }

                this.barcodes.Clear();
                foreach (var barcode in value)
                {
                    this.barcodes.Add(barcode);
                }
            }
        }

        public void BeginEdit()
        {
            if (!this.isInEditMode)
            {
                this.tempProduct = this.MemberwiseClone() as SimplifiedProduct;
                this.tempBarcodes = new List<Barcode>(this.barcodes);
                this.isInEditMode = true;
            }
        }

        public void EndEdit()
        {
            if (this.isInEditMode)
            {
                this.tempProduct = null;
                this.tempBarcodes = null;
                this.isInEditMode = false;
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}