namespace Eugenie.Clients.Admin.Helpers
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using Common.Models;

    using Contracts;

    using Models;

    using Newtonsoft.Json;

    using Properties;

    using Store = Models.Store;

    public class Storage : IServerStorage, ITasksStorage
    {
        public Storage()
        {
            this.Servers = JsonConvert.DeserializeObject<ObservableCollection<Store>>(Settings.Default.Servers);
            this.AddOrUpdateProductTasks = JsonConvert.DeserializeObject<ObservableCollection<AddOrUpdateProductTask>>(Settings.Default.AddOrUpdateProductTasks);
            this.DeleteProductTasks = JsonConvert.DeserializeObject<ObservableCollection<DeleteProductTask>>(Settings.Default.DeleteProductTasks);

            this.Servers.CollectionChanged += this.OnServersChanged;
            this.AddOrUpdateProductTasks.CollectionChanged += this.OnAddOrUpdateProductTasksChanged;
            this.DeleteProductTasks.CollectionChanged += this.OnDeleteProductTasksChanged;
        }

        public ObservableCollection<Store> Servers { get; }

        public ObservableCollection<AddOrUpdateProductTask> AddOrUpdateProductTasks { get; }

        public ObservableCollection<DeleteProductTask> DeleteProductTasks { get; }

        private void OnServersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Settings.Default.Servers = JsonConvert.SerializeObject(this.Servers);
            Settings.Default.Save();
        }

        private void OnAddOrUpdateProductTasksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Settings.Default.AddOrUpdateProductTasks = JsonConvert.SerializeObject(this.AddOrUpdateProductTasks);
            Settings.Default.Save();
        }

        private void OnDeleteProductTasksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Settings.Default.DeleteProductTasks = JsonConvert.SerializeObject(this.DeleteProductTasks);
            Settings.Default.Save();
        }
    }
}