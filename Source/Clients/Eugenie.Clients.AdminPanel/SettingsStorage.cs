namespace Eugenie.Clients.AdminPanel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;
    using Common.Models;

    using Newtonsoft.Json;

    using Properties;

    public class SettingsStorage : IServerStorage
    {
        private ObservableCollection<ServerInformation> servers;

        public ICollection<ServerInformation> Servers
        {
            get
            {
                if (this.servers == null)
                {
                    if (Settings.Default.Servers == string.Empty)
                    {
                        Settings.Default.Servers = "[]";
                        Settings.Default.Save();
                    }

                    this.servers = JsonConvert.DeserializeObject<ObservableCollection<ServerInformation>>(Settings.Default.Servers);
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

        public event EventHandler<ServerAddedEventArgs> ServerAdded;

        public void AddServer(ServerInformation server)
        {
            this.Servers.Add(server);
            this.SaveSettings();
            
            this.ServerAdded?.Invoke(this, new ServerAddedEventArgs(server));
        }

        public event EventHandler<ServerDeletedEventArgs> ServerDeleted;

        public void DeleteServer(ServerInformation server)
        {
            var serverToDelete = this.Servers.FirstOrDefault(x => x.Name == server.Name);
            this.Servers.Remove(serverToDelete);
            this.SaveSettings();

            this.ServerDeleted?.Invoke(this, new ServerDeletedEventArgs(server));
        }

        private void SaveSettings()
        {
            Settings.Default.Servers = JsonConvert.SerializeObject(this.Servers);
            Settings.Default.Save();
        }
    }
}