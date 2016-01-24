namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight.CommandWpf;

    using MaterialDesignThemes.Wpf;

    public class ReportDetailsViewModel
    {
        private readonly IWebApiClient apiClient;

        public ReportDetailsViewModel(IWebApiClient apiClient, DateTime date, HttpClient client)
        {
            this.apiClient = apiClient;

            this.Cancel = new RelayCommand(this.HandleCancel);
            this.Waste = new ObservableCollection<Waste>();
            this.Sells = new ObservableCollection<Sell>();
            this.Shipments = new ObservableCollection<Shipment>();
            this.Initialize(client, date);
        }

        public ICommand Cancel { get; set; }

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
    }
}