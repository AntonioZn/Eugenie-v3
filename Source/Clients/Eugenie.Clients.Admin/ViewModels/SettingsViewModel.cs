﻿namespace Eugenie.Clients.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Models;
    using Common.Notifications;

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Models;

    public class SettingsViewModel : ViewModelBase, IKeyHandler
    {
        private readonly IServerStorage storage;
        private readonly ITasksStorage tasksStorage;

        public SettingsViewModel(IServerStorage storage, ITasksStorage tasksStorage)
        {
            this.storage = storage;
            this.tasksStorage = tasksStorage;

            this.NewServerViewModel = new NewServerViewModel();

            this.Add = new RelayCommand(this.HandleAdd, this.CanAdd);
            this.Delete = new RelayCommand<ServerInformation>(this.HandleDelete);
        }

        public NewServerViewModel NewServerViewModel { get; set; }

        public ICommand Add { get; }

        public ICommand Delete { get; }

        public ICollection<AddOrUpdateProductTask> AddOrUpdateProductTasks => this.tasksStorage.AddOrUpdateProductTasks;

        public AddOrUpdateProductTask SelectedTask { get; set; }

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

        public ICollection<ServerInformation> Servers => this.storage.Servers;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private bool CanAdd()
        {
            return this.NewServerViewModel.HasNoValidationErrors()
                   && this.storage.Servers.All(x => x.Name != this.NewServerViewModel.Name && x.Addresses.All(y => this.NewServerViewModel.AddressesArray.All(t => t != y)));
        }

        public void HandleAdd()
        {
            this.storage.Servers.Add(this.NewServerViewModel.GetServer());
            this.NewServerViewModel.Reset();
        }

        private void HandleDelete(ServerInformation server)
        {
            this.storage.Servers.Remove(server);
        }
    }
}