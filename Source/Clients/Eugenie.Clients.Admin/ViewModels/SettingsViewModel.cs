namespace Eugenie.Clients.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;

    using Common.Models;

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IServerStorage storage;

        public SettingsViewModel(IServerStorage storage)
        {
            this.storage = storage;

            this.NewServerViewModel = new NewServerViewModel();

            this.Add = new RelayCommand(this.HandleAdd, this.CanAdd);
            this.Delete = new RelayCommand<ServerInformation>(this.HandleDelete);
        }

        public NewServerViewModel NewServerViewModel { get; set; }

        public ICommand Add { get; }

        public ICommand Delete { get; }

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