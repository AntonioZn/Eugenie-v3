namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    public class SellersViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private ObservableCollection<Seller> sellers; 

        public SellersViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedServerChanged += this.OnSelectedServerChanged;
        }

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

        private void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.Sellers = this.manager.Cache.SellersPerServer[this.manager.SelectedServer];
        }
    }
}
