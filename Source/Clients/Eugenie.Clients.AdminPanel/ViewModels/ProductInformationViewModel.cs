namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Common.Models;

    using Data.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Product = Common.Models.Product;

    public class ProductInformationViewModel : ViewModelBase
    {
        private DateTime? date;
        private string batch;

        public ProductInformationViewModel(IDictionary<ServerInformation, Product> products, SimplifiedProduct simpleProduct)
        {
            this.Products = products;
            this.FirstProduct = simpleProduct;

            this.Measures = new List<MeasureType>();
            foreach (var measureType in Enum.GetValues(typeof(MeasureType)))
            {
                this.Measures.Add((MeasureType)measureType);
            }


            this.DeleteBarcodeCommand = new RelayCommand<Barcode>(this.HandleDeleteBarcodeCommand);
            this.AddExpirationDateCommand = new RelayCommand<ServerInformation>(this.HandleAddExpirationDateCommand);
        }

        public ICommand AddExpirationDateCommand { get; set; }

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

        //TODO: add batch text field to view
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

        public ICommand DeleteBarcodeCommand { get; set; }

        public IDictionary<ServerInformation, Product> Products { get; set; }

        public SimplifiedProduct FirstProduct { get; set; }

        public ICollection<MeasureType> Measures { get; set; }

        private void HandleDeleteBarcodeCommand(Barcode barcode)
        {
            this.FirstProduct.Barcodes.Remove(barcode);
        }

        private void HandleAddExpirationDateCommand(ServerInformation server)
        {
            this.Products[server].ExpirationDates.Add(new ExpirationDate { Batch = this.Batch, Date = this.Date.GetValueOrDefault() });
        }
    }
}