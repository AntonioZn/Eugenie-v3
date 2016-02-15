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

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class SellersViewModel : ViewModelBase
    {
        private readonly IWebApiClient apiClient;
        private readonly IServerManager manager;
        private ObservableCollection<Seller> sellers;
        private decimal sellsTotal;
        private decimal wasteTotal;

        public SellersViewModel(IServerManager manager, IWebApiClient apiClient)
        {
            this.manager = manager;
            this.manager.SelectedServerChanged += this.OnSelectedServerChanged;
            this.apiClient = apiClient;

            this.Waste = new ObservableCollection<Waste>();
            this.Sells = new ObservableCollection<Sell>();

            this.Search = new RelayCommand(this.HandleSearch, this.CanSearch);
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

        public DateTime Start { get; set; } = DateTime.Now;

        public DateTime End { get; set; } = DateTime.Now;

        public ObservableCollection<Waste> Waste { get; }

        public ObservableCollection<Sell> Sells { get; }

        public decimal SellsTotal
        {
            get
            {
                return this.sellsTotal;
            }
            set
            {
                this.Set(() => this.SellsTotal, ref this.sellsTotal, value);
            }
        }

        public decimal WasteTotal
        {
            get
            {
                return this.wasteTotal;
            }
            set
            {
                this.Set(() => this.WasteTotal, ref this.wasteTotal, value);
            }
        }

        private async void HandleSearch()
        {
            var deals = await this.apiClient.GetDealsForSeller(this.manager.SelectedServer.Client, this.SelectedSeller.UserName, this.Start, this.End);

            this.Waste.Clear();
            deals.Waste.ForEach(this.Waste.Add);
            this.WasteTotal = deals.Waste.Sum(x => x.Total);

            this.Sells.Clear();
            deals.Sells.ForEach(this.Sells.Add);
            this.SellsTotal = deals.Sells.Sum(x => x.Total);
        }

        private bool CanSearch()
        {
            return this.SelectedSeller != null;
        }

        private void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.Sellers = this.manager.SelectedServer == null ? Enumerable.Empty<Seller>() : this.manager.Cache.SellersPerServer[this.manager.SelectedServer];
        }
    }
}