namespace Eugenie.Clients.AdminPanel
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;
    using Common.Models;

    using Newtonsoft.Json;

    using Properties;

    public class SettingsStorage : IServerStorage
    {
        private ObservableCollection<ServerInformation> servers;

        public ObservableCollection<ServerInformation> Servers
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
                this.servers = this.servers ?? (this.servers = new ObservableCollection<ServerInformation>());

                this.servers.Clear();
                foreach (var server in value)
                {
                    this.servers.Add(server);
                }
            }
        }

        public void AddServer(ServerInformation server)
        {
            this.Servers.Add(server);
            this.SaveSettings();
        }

        public void DeleteServer(ServerInformation server)
        {
            var serverToDelete = this.Servers.FirstOrDefault(x => x.Name == server.Name);
            this.Servers.Remove(serverToDelete);
            this.SaveSettings();
        }

        private void SaveSettings()
        {
            Settings.Default.Servers = JsonConvert.SerializeObject(this.Servers);
            Settings.Default.Save();
        }
    }
}