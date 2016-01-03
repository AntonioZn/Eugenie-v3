namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Newtonsoft.Json;

    using Properties;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private ObservableCollection<ServerInformation> servers; 
        private string name;
        private string address = "http://";
        private string username;
        private string password;

        public SettingsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.AddNewServerCommand = new RelayCommand(this.HandleAddNewServerCommand);
            this.DeleteServerCommand = new RelayCommand<ServerInformation>(this.HandleDeleteServerCommand);
            var savedServers = JsonConvert.DeserializeObject<ICollection<ServerInformation>>(Settings.Default.Servers);
            if (savedServers != null)
            {
                this.Servers = savedServers;
            }
        }

        public ICommand AddNewServerCommand { get; private set; }
        public ICommand DeleteServerCommand { get; private set; }

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


            this.manager.AddServer(newServer);
        }

        private void HandleDeleteServerCommand(ServerInformation server)
        {
            this.Servers.Remove(server);

            this.manager.DeleteServer(server);
        }
    }
}
