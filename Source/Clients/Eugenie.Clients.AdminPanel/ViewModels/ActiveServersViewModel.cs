namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
            this.RefreshServersCommand = new RelayCommand(this.Refresh);
            this.LoadingVisibility = Visibility.Collapsed;

            manager.ServerTestingFinished += this.OnServerTestingFinished;
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
                if (this.servers == null)
                {
                    this.servers = new ObservableCollection<ServerInformation>();
                }

                return this.servers;
            }
            set
            {
                if (this.servers == null)
                {
                    this.servers = new ObservableCollection<ServerInformation>();
                }

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
            this.Servers = this.manager.ActiveServers.Keys;
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}