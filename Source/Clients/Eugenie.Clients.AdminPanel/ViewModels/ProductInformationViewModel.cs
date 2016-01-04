﻿namespace Eugenie.Clients.AdminPanel.ViewModels
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
            this.DeleteExpirationDateCommand = new RelayCommand<object>(this.HandleDeleteExpirationDateCommand);
            this.AddExpirationDateCommand = new RelayCommand<Product>(this.HandleAddExpirationDateCommand);
        }

        public ICommand DeleteExpirationDateCommand { get; set; }

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

        //TODO: add validation
        private void HandleAddExpirationDateCommand(Product product)
        {
            product.ExpirationDates.Add(new ExpirationDate { Batch = this.Batch, Date = this.Date.GetValueOrDefault() });
            this.Date = null;
            this.Batch = null;
        }

        private void HandleDeleteExpirationDateCommand(object parameter)
        {
            var values = (object[])parameter;
            var expirationDate = values[0] as ExpirationDate;
            var product = values[1] as Product;
            product.ExpirationDates.Remove(expirationDate);
        }
    }
}