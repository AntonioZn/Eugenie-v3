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
        private string address;
        private bool isSelfHost;
        private string port;

        public SettingsViewModel()
        {
            this.Address = SettingsManager.Default.Settings.Address;
            this.Port = SettingsManager.Default.Settings.Port.ToString();
            this.IsSelfHost = SettingsManager.Default.Settings.IsSelfHost;

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
            SettingsManager.Default.Settings.Address = this.Address;
            SettingsManager.Default.Settings.Port = int.Parse(this.Port);
            SettingsManager.Default.Settings.IsSelfHost = this.IsSelfHost;
            SettingsManager.Default.Save();

            var mainWindowViewModel = ViewModelLocator.container.Resolve<MainWindowViewModel>();
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
            BackupDatabaseService.Backup(SettingsManager.Default.Settings.BackupPath);
        }

        private void HandleCancel()
        {
            ViewModelLocator.container.Resolve<MainWindowViewModel>().ShowLogin();
        }
    }
}