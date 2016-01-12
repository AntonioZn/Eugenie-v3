namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IServerStorage storage;
        private ServerInformation newServer;

        public SettingsViewModel(IServerStorage storage)
        {
            this.storage = storage;
            this.AddNewServerCommand = new RelayCommand(this.HandleAddNewServerCommand, this.CanAddNewServer);
            this.DeleteServerCommand = new RelayCommand<ServerInformation>(this.HandleDeleteServerCommand);
        }
        
        public ICommand AddNewServerCommand { get; private set; }

        public ICommand DeleteServerCommand { get; private set; }

        public ServerInformation NewServer
        {
            get
            {
                return this.newServer ?? (this.newServer = new ServerInformation());
            }
            set
            {
                this.Set(() => this.NewServer, ref this.newServer, value);
            }
        }

        public ICollection<ServerInformation> Servers => this.storage.Servers;

        private bool CanAddNewServer()
        {
            return this.NewServer.HasNoValidationErrors()
                && this.storage.Servers.FirstOrDefault(x => x.Name == this.NewServer.Name || x.Address == this.NewServer.Address) == null;
        }

        public void HandleAddNewServerCommand()
        {
            this.storage.AddServer(this.NewServer);

            this.NewServer = new ServerInformation();;
        }

        private void HandleDeleteServerCommand(ServerInformation server)
        {
            this.storage.DeleteServer(server);
        }
    }
}