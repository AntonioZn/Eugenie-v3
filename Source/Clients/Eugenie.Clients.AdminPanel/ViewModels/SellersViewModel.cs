namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class SellersViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private readonly IWebApiClient apiClient;
        private ObservableCollection<Seller> sellers;
        private DateTime? start;
        private DateTime? end;

        public SellersViewModel(IServerManager manager, IWebApiClient apiClient)
        {
            this.manager = manager;
            this.manager.SelectedServerChanged += this.OnSelectedServerChanged;
            this.apiClient = apiClient;

            this.Waste = new ObservableCollection<Waste>();
            this.Sells = new ObservableCollection<Sell>();

            this.Search = new RelayCommand(this.HandleSearch, this.CanSearch);
        }

        private async void HandleSearch()
        {
            var deals = await this.apiClient.GetDealsForSeller(this.manager.SelectedServer.Client, this.SelectedSeller.UserName, this.Start.Value, this.End.Value);
            
            this.Waste.Clear();
            deals.Waste.ForEach(this.Waste.Add);

            this.Sells.Clear();
            deals.Sells.ForEach(this.Sells.Add);
        }

        private bool CanSearch()
        {
            return this.SelectedSeller != null && this.Start != null && this.End != null;
        }

        public ICommand Search { get; }

        public Seller SelectedSeller { get; set; }

        public IEnumerable<Seller> Sellers
        {
            get
            {
                return this.sellers ?? (this.sellers = new ObservableCollection<Seller>());
            }

            set
            {
                this.sellers = this.sellers ?? new ObservableCollection<Seller>();
                this.sellers.Clear();
                value.ForEach(this.sellers.Add);
            }
        }

        public DateTime? Start
        {
            get
            {
                return this.start;
            }

            set
            {
                this.Set(() => this.Start, ref this.start, value);
            }
        }

        public DateTime? End
        {
            get
            {
                return this.end;
            }

            set
            {
                this.Set(() => this.End, ref this.end, value);
            }
        }

        public ObservableCollection<Waste> Waste { get; }

        public ObservableCollection<Sell> Sells { get; }

        private void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.Sellers = this.manager.Cache.SellersPerServer[this.manager.SelectedServer];
        }
    }
}
