namespace Eugenie.Clients.Common.Models
{
    using System.Collections.Generic;
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

        public SimplifiedProduct(int id, string name, decimal buyingPrice, MeasureType measure, IEnumerable<Barcode> barcodes)
        {
            this.Id = id;
            this.Name = name;
            this.BuyingPrice = buyingPrice;
            this.Measure = measure;
            this.Barcodes = barcodes;
        }

        public int Id { get; set; }

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

        public IEnumerable<Barcode> Barcodes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BeginEdit()
        {
            if (!this.isInEditMode)
            {
                this.tempProduct = this.MemberwiseClone() as SimplifiedProduct;
                this.isInEditMode = true;
            }
        }

        public void EndEdit()
        {
            if (this.isInEditMode)
            {
                this.tempProduct = null;
                this.isInEditMode = false;
            }
        }

        public void CancelEdit()
        {
            if (this.isInEditMode)
            {
                this.Id = this.tempProduct.Id;
                this.Name = this.tempProduct.Name;
                this.BuyingPrice = this.tempProduct.BuyingPrice;
                this.Measure = this.tempProduct.Measure;
                this.isInEditMode = false;
            }
        }
    }
}