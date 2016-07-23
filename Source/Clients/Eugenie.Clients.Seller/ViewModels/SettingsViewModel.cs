namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Windows.Input;

    using Autofac;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Helpers;

    using Server.Host;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsManager settingsManager;
        private string address;
        private bool isSelfHost;
        private string port;

        public SettingsViewModel(SettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            this.Address = this.settingsManager.Settings.Address;
            this.Port = this.settingsManager.Settings.Port.ToString();
            this.IsSelfHost = this.settingsManager.Settings.IsSelfHost;

            this.Save = new RelayCommand(this.HandleSave);
            this.Backup = new RelayCommand(this.HandleBackup);
            this.Cancel = new RelayCommand(this.HandleCancel);
        }

        public ICommand Save { get; }

        public ICommand Backup { get; }

        public ICommand Cancel { get; }

        public string Port
        {
            get
            {
                return this.port;
            }

            set
            {
                int parsedPort;
                if (int.TryParse(value, out parsedPort))
                {
                    this.Set(() => this.Port, ref this.port, value);
                    if (this.IsSelfHost)
                    {
                        this.Address = this.GetAddress();
                    }
                }
            }
        }

        public bool IsSelfHost
        {
            get
            {
                return this.isSelfHost;
            }

            set
            {
                this.Set(() => this.IsSelfHost, ref this.isSelfHost, value);
                if (this.isSelfHost)
                {
                    this.Address = this.GetAddress();
                }
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

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private void HandleSave()
        {
            this.settingsManager.Settings.Address = this.Address;
            this.settingsManager.Settings.Port = int.Parse(this.Port);
            this.settingsManager.Settings.IsSelfHost = this.IsSelfHost;
            this.settingsManager.Save();

            var mainWindowViewModel = ViewModelLocator.Container.Resolve<MainWindowViewModel>();
            mainWindowViewModel.Initialize();
            mainWindowViewModel.ShowLogin();
        }

        private string GetAddress()
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var selfHostAddress = "http://" + localIp + ":" + this.port;
            return selfHostAddress;
        }

        private void HandleBackup()
        {
            BackupDatabaseService.Backup(this.settingsManager.Settings.BackupPath);
        }

        private void HandleCancel()
        {
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowLogin();
        }
    }
}