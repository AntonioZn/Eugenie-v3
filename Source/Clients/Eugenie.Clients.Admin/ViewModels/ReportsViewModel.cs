namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Helpers;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ReportsViewModel : ViewModelBase, IKeyHandler
    {
        private readonly ServerManager manager;
        private readonly ObservableCollection<Report> reports = new ObservableCollection<Report>();
        private decimal? currentStock;

        public ReportsViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedStoreChanged += this.OnSelectedStoreChanged;
        }

        public ICommand OpenDetailsCommand => new RelayCommand(this.OpenDetails);

        public Report SelectedReport { get; set; }

        public IEnumerable<Report> Reports
        {
            get => this.reports;
            set
            {
                this.reports.Clear();
                value.ForEach(this.reports.Add);
            }
        }

        public decimal? CurrentStock
        {
            get => this.currentStock;
            set
            {
                this.Set(() => this.CurrentStock, ref this.currentStock, value);
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    this.OpenDetails();
                    e.Handled = true;
                    break;
            }
        }

        public async void OpenDetails()
        {
            var viewModel = new ReportDetailsViewModel(this.manager.SelectedStore, this.SelectedReport.Date);
            await DialogHost.Show(new ReportDetails(viewModel), "RootDialog");
        }

        private void OnSelectedStoreChanged(object sender, EventArgs e)
        {
            this.Reports = this.manager.SelectedStore?.Reports ?? Enumerable.Empty<Report>();
            this.CurrentStock = this.manager.SelectedStore?.Products.Sum(x => x.Quantity * x.SellingPrice) ?? 0;
        }
    }
}