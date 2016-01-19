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
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class ActiveServersViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private Visibility loadingVisibility;
        private ObservableCollection<ActiveServer> servers;

        public ActiveServersViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.LoadingVisibility = Visibility.Visible;

            manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.Refresh = new RelayCommand(this.HandleRefresh);
            this.Select = new RelayCommand<ActiveServer>(this.HandleSelect);
        }

        public ICommand Refresh { get; }

        public ICommand Select { get; }

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

        public ICollection<ActiveServer> Servers
        {
            get
            {
                return this.servers ?? (this.servers = new ObservableCollection<ActiveServer>());
            }
            set
            {
                this.servers = this.servers ?? new ObservableCollection<ActiveServer>();
                this.servers.Clear();
                value.ForEach(this.servers.Add);
            }
        }

        private void HandleRefresh()
        {
            this.LoadingVisibility = Visibility.Visible;
            this.manager.Initialize();
        }

        private void HandleSelect(ActiveServer selectedServer)
        {
            foreach (var server in this.Servers)
            {
                server.IsSelected = false;
            }

            selectedServer.IsSelected = true;

            this.manager.SelectedServer = selectedServer;
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.Servers = this.manager.Cache.ProductsPerServer.Keys.Where(x => x.Client != null).Select(x => new ActiveServer(x.Name)).ToList();
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}