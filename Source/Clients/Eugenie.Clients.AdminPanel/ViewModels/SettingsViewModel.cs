namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IServerStorage storage;
        private string name;
        private string address = "http://";
        private string username;
        private string password;

        public SettingsViewModel(IServerStorage storage)
        {
            this.storage = storage;
            this.AddNewServerCommand = new RelayCommand(this.HandleAddNewServerCommand);
            this.DeleteServerCommand = new RelayCommand<ServerInformation>(this.HandleDeleteServerCommand);
        }

        public ICommand AddNewServerCommand { get; private set; }
        public ICommand DeleteServerCommand { get; private set; }

        public ICollection<ServerInformation> Servers => this.storage.Servers;

        public string Name
        {
            get
            {
                return this.name; 
                
            }
            set
            {
                this.Set(() => this.Name, ref this.name, value);
            }
        }

        public string Address
        {
            get
            {
                return this.address;

            }
            set
            {
                this.Set(() => this.Address, ref this.address, value);
            }
        }

        public string Username
        {
            get
            {
                return this.username;

            }
            set
            {
                this.Set(() => this.Username, ref this.username, value);
            }
        }

        public string Password
        {
            get
            {
                return this.password;

            }
            set
            {
                this.Set(() => this.Password, ref this.password, value);
            }
        }

        private void HandleAddNewServerCommand()
        {
            var newServer = new ServerInformation(this.username, this.password, this.name, this.address);

            this.Name = string.Empty;
            this.Address = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;


            this.storage.AddServer(newServer);
        }

        private void HandleDeleteServerCommand(ServerInformation server)
        {
            this.Servers.Remove(server);

            this.storage.DeleteServer(server);
        }
    }
}
