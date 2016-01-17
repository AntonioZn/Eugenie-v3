namespace Eugenie.Clients.AdminPanel
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using Common.Contracts;
    using Common.Models;

    using Newtonsoft.Json;

    using Properties;

    public class SettingsStorage : IServerStorage
    {
        public SettingsStorage()
        {
            if (string.IsNullOrEmpty(Settings.Default.Servers))
            {
                Settings.Default.Servers = "[]";
                Settings.Default.Save();
            }
            this.Servers = JsonConvert.DeserializeObject<ObservableCollection<ServerInformation>>(Settings.Default.Servers);
            this.Servers.CollectionChanged += this.OnServersChanged;
        }

        public ObservableCollection<ServerInformation> Servers { get; }

        private void OnServersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Settings.Default.Servers = JsonConvert.SerializeObject(this.Servers);
            Settings.Default.Save();
        }
    }
}