namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Input;

    using Helpers;

    using Models;

    using Server.Host;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Mvvm;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly Settings settings;
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly LotteryTicketChecker lotteryTicketChecker;
        private string address;
        private bool isSelfHost;
        private string port;

        public SettingsViewModel(Settings settings, MainWindowViewModel mainWindowViewModel, LotteryTicketChecker lotteryTicketChecker)
        {
            this.settings = settings;
            this.mainWindowViewModel = mainWindowViewModel;
            this.lotteryTicketChecker = lotteryTicketChecker;

            this.Address = this.settings.Address;
            this.Port = this.settings.Port.ToString();
            this.IsSelfHost = this.settings.IsSelfHost;

            this.Save = new RelayCommand(this.HandleSave);
            this.Backup = new RelayCommand(() => BackupDatabaseService.Backup(this.settings.BackupPath));
            this.Cancel = new RelayCommand(() => this.mainWindowViewModel.ShowLogin());
            this.LotteryLogin = new RelayCommand(this.HandleLotteryLogin);
        }

        public ICommand Save { get; }

        public ICommand Backup { get; }

        public ICommand Cancel { get; }

        public ICommand LotteryLogin { get; }

        public string Port
        {
            get => this.port;

            set
            {
                int parsedPort;
                if (int.TryParse(value, out parsedPort))
                {
                    this.Set(ref this.port, value);
                    if (this.IsSelfHost)
                    {
                        this.Address = this.GetAddress();
                    }
                }
            }
        }

        public bool IsSelfHost
        {
            get => this.isSelfHost;

            set
            {
                this.Set(ref this.isSelfHost, value);
                if (this.isSelfHost)
                {
                    this.Address = this.GetAddress();
                }
            }
        }

        public string Address
        {
            get => this.address;

            set => this.Set(ref this.address, value);
        }

        private void HandleSave()
        {
            this.settings.Address = this.Address;
            this.settings.Port = int.Parse(this.Port);
            this.settings.IsSelfHost = this.IsSelfHost;
            this.settings.Save();
            
            this.mainWindowViewModel.Initialize();
            this.mainWindowViewModel.ShowLogin();
        }

        private string GetAddress()
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var selfHostAddress = "http://" + localIp + ":" + this.port;
            return selfHostAddress;
        }

        //TODO: add better error messages
        private async void HandleLotteryLogin()
        {
            var result = await this.lotteryTicketChecker.Login(this.settings.LotteryUsername, this.settings.LotteryPassword);
            if (result)
            {
                NotificationsHost.Success("Notifications", "Успешно", "Името и паролата са валидни");
            }
            else
            {
                NotificationsHost.Error("Notifications", "Грешка", "Грешно име или парола");
            }
        }
    }
}