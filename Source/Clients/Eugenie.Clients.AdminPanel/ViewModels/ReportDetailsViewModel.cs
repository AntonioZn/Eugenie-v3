namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Helpers;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    public class ReportDetailsViewModel : ViewModelBase
    {
        private readonly IWebApiClient apiClient;
        private readonly DateTime date;

        public ReportDetailsViewModel(IWebApiClient apiClient, DateTime date, HttpClient client)
        {
            this.apiClient = apiClient;
            this.date = date;

            this.Waste = new ObservableCollection<Waste>();
            this.Sells = new ObservableCollection<Sell>();
            this.Shipments = new ObservableCollection<Shipment>();

            this.Cancel = new RelayCommand(this.HandleCancel);
            this.ShowPdf = new RelayCommand(this.HandleShowPdf, this.CanShowPdf);

            this.Initialize(client, date);
        }

        public ICommand Cancel { get; set; }

        public ICommand ShowPdf { get; set; }

        public ObservableCollection<Waste> Waste { get; }

        public ObservableCollection<Sell> Sells { get; }

        public ObservableCollection<Shipment> Shipments { get; }

        private async void Initialize(HttpClient client, DateTime date)
        {
            var details = await this.apiClient.GetReportDetailsAsync(client, date);
            
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