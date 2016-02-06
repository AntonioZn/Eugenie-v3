namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Input;

    using Autofac;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Views;

    using Settings = Properties.Settings;

    public class SettingsViewModel : ViewModelBase
    {
        private string address;
        private string port;
        private bool isSelfHost;

        public SettingsViewModel()
        {
            this.Address = Settings.Default.Address;
            this.Port = Settings.Default.Port.ToString();
            this.IsSelfHost = Settings.Default.IsSelfHost;

            this.Start = new RelayCommand(this.HandleStart);
            this.Save = new RelayCommand(this.HandleSave);
        }

        public ICommand Start { get; set; }

        public ICommand Save { get; set; }

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

        private void HandleStart()
        {
            MainWindowViewModel.HostServer(int.Parse(this.Port));
        }

        private void HandleSave()
        {
            Settings.Default.Address = this.Address;
            Settings.Default.Port = int.Parse(this.Port);
            Settings.Default.IsSelfHost = this.IsSelfHost;
            Settings.Default.Save();

            ViewModelLocator.container.Resolve<MainWindowViewModel>().Content = new Login();
        }

        private string GetAddress()
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var selfHostAddress = "http://" + localIp + ":" + port;
            return selfHostAddress;
        }
    }
}
