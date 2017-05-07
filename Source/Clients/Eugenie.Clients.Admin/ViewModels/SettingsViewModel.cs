namespace Eugenie.Clients.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Notifications;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Helpers;

    using Models;

    public class SettingsViewModel : ViewModelBase, IKeyHandler
    {
        private readonly ServerManager serverManager;
        private readonly TaskManager taskManager;
        private Store newStore = new Store();

        public SettingsViewModel(ServerManager serverManager, TaskManager taskManager)
        {
            this.serverManager = serverManager;
            this.taskManager = taskManager;
        }

        public Store NewStore
        {
            get => this.newStore;
            set
            {
                this.Set(() => this.NewStore, ref this.newStore, value);
            }
        }

        public ICommand AddCommand => new RelayCommand(this.Add, this.CanAdd);

        public ICommand DeleteCommand => new RelayCommand<Store>(this.Delete);

        public ICollection<AddOrUpdateProductTask> AddOrUpdateProductTasks => this.taskManager.AddOrUpdateProductTasks;

        public AddOrUpdateProductTask SelectedTask { get; set; }

        public ICollection<Store> Stores => this.serverManager.Stores;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Delete:
                    this.DeleteTask();
                    e.Handled = true;
                    break;
            }
        }

        private bool CanAdd()
        {
            return this.NewStore.HasNoValidationErrors()
                   && this.serverManager.Stores.All(x => x.Name != this.NewStore.Name && x.Address != this.NewStore.Address);
        }

        private void Add()
        {
            this.serverManager.Stores.Add(this.NewStore);
            this.NewStore = new Store();
        }

        //TODO: delete all tasks
        private void Delete(Store server)
        {
            this.serverManager.Stores.Remove(server);
        }

        //TODO: Handle collection changed exception
        private void DeleteTask()
        {
            if (this.SelectedTask != null)
            {
                this.AddOrUpdateProductTasks.Remove(this.SelectedTask);
            }
            else
            {
                NotificationsHost.Error("Неуспешно", "Трябва да има избрана заявка.");
            }
        }
    }
}