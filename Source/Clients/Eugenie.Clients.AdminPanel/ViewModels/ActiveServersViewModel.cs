namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;

    public class ActiveServersViewModel : ViewModelBase
    {
        private readonly IServersManager serversManager;
        private Visibility loadingVisibility;
        private ObservableCollection<ServerInformation> servers;

        public ActiveServersViewModel(IServersManager serversManager)
        {
            this.serversManager = serversManager;
            this.LoadingVisibility = Visibility.Collapsed;
            this.Initialize();
        }

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

        private async void Initialize()
        {
            this.LoadingVisibility = Visibility.Visible;
            var serversDictionary = await this.serversManager.TestServers();
            this.Servers = serversDictionary.Keys;
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}