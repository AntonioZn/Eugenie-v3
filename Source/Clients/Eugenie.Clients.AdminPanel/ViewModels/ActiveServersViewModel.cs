namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class ActiveServersViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private Visibility loadingVisibility;
        private ObservableCollection<ServerInformation> servers;

        public ActiveServersViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.LoadingVisibility = Visibility.Visible;

            manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.RefreshServersCommand = new RelayCommand(this.Refresh);
        }

        public ICommand RefreshServersCommand { get; private set; }

        public Visibility LoadingVisibility
        {
            get
            {
                return this.loadingVisibility;
            }

            set
            {
                this.Set(() => this.LoadingVisibility, ref this.loadingVisibility, value);
            }
        }
        
        public ICollection<ServerInformation> Servers
        {
            get
            {
                return this.servers ?? (this.servers = new ObservableCollection<ServerInformation>());
            }
            set
            {
                this.servers = this.servers ?? new ObservableCollection<ServerInformation>();
                this.servers.Clear();
                foreach (var server in value)
                {
                    this.servers.Add(server);
                }
            }
        }

        private void Refresh()
        {
            this.LoadingVisibility = Visibility.Visible;
            this.manager.Initialize();
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.Servers = this.manager.Cache.ProductsPerServer.Keys.Where(x => x.Client != null).ToList();
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}