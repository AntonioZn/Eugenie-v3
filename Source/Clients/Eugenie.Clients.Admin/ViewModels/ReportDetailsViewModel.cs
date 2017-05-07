namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Models;

    public class ReportDetailsViewModel : ViewModelBase
    {
        private readonly Store store;
        private readonly DateTime date;

        public ReportDetailsViewModel(Store store, DateTime date)
        {
            this.store = store;
            this.date = date;

            this.Waste = new ObservableCollection<Waste>();
            this.Sells = new ObservableCollection<Sell>();
            this.Shipments = new ObservableCollection<Shipment>();

            this.Cancel = new RelayCommand(this.HandleCancel);
            this.ShowPdf = new RelayCommand(this.HandleShowPdf, this.CanShowPdf);

            this.Initialize();
        }

        public ICommand Cancel { get; set; }

        public ICommand ShowPdf { get; set; }

        public ObservableCollection<Waste> Waste { get; }

        public ObservableCollection<Sell> Sells { get; }

        public ObservableCollection<Shipment> Shipments { get; }

        private async void Initialize()
        {
            var details = await this.store.Client.GetReportDetailsAsync(this.date);

            this.Waste.Clear();
            details.Waste.ForEach(this.Waste.Add);

            this.Sells.Clear();
            details.Sells.ForEach(this.Sells.Add);

            this.Shipments.Clear();
            details.Shipments.ForEach(this.Shipments.Add);
        }

        private void HandleCancel()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private bool CanShowPdf()
        {
            return this.Shipments.Any();
        }

        private void HandleShowPdf()
        {
            new PdfShipmentInvoiceGenerator(this.date, this.Shipments);
        }
    }
}