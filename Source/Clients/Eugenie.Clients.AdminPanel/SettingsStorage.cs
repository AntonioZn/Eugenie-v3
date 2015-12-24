namespace Eugenie.Clients.AdminPanel
{
    using System.Collections.Generic;

    using Common.Contracts;
    using Common.Models;

    using Newtonsoft.Json;

    using Properties;

    public class SettingsStorage : IServerStorage
    {
        public SettingsStorage()
        {
            if (Settings.Default.Servers == string.Empty)
            {
                Settings.Default.Servers = "[]";
                Settings.Default.Save();
            }

            this.Servers = JsonConvert.DeserializeObject<ICollection<ServerInformation>>(Settings.Default.Servers);
        }

        public ICollection<ServerInformation> Servers { get; set; }

        public void AddServer(ServerInformation server)
        {
            this.Servers.Add(server);
            this.SaveSettings();

        }

        public void DeleteServer(ServerInformation server)
        {
            this.Servers.Remove(server);
            this.SaveSettings();
        }

        private void SaveSettings()
        {
            Settings.Default.Servers = JsonConvert.SerializeObject(this.Servers);
            Settings.Default.Save();
        }
    }
}