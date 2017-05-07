namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    
    using Common.WebApiModels;
    using Common.Еxtensions;
    
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    public class SellersViewModel : ViewModelBase
    {
        private readonly ServerManager manager;
        private readonly ObservableCollection<Seller> sellers = new ObservableCollection<Seller>();
        private decimal sellsTotal;
        private decimal wasteTotal;

        public SellersViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedStoreChanged += this.OnSelectedStoreChanged;

            this.Waste = new ObservableCollection<Waste>();
            this.Sells = new ObservableCollection<Sell>();
        }

        public ICommand SearchCommand => new RelayCommand(this.Search, this.CanSearch);

        public Seller SelectedSeller { get; set; }

        public IEnumerable<Seller> Sellers
        {
            get => this.sellers;
            set
            {
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
            get => this.sellsTotal;
            set
            {
                this.Set(() => this.SellsTotal, ref this.sellsTotal, value);
            }
        }

        public decimal WasteTotal
        {
            get => this.wasteTotal;
            set
            {
                this.Set(() => this.WasteTotal, ref this.wasteTotal, value);
            }
        }

        private async void Search()
        {
            var deals = await this.manager.SelectedStore.Client.GetDealsForSeller(this.SelectedSeller.Username, this.Start, this.End);

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

        private void OnSelectedStoreChanged(object sender, EventArgs e)
        {
            this.Sellers = this.manager.SelectedStore?.Sellers ?? Enumerable.Empty<Seller>();
        }
    }
}