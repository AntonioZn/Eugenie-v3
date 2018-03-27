namespace Eugenie.Clients.Seller.ViewModels
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Input;

    using Autofac;

    using Helpers;

    using Models;

    using Server.Host;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Mvvm;
    using Sv.Wpf.Core.Mvvm.ValidationRules;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly Settings settings;
        private readonly LotteryTicketChecker lotteryTicketChecker;
        private bool isSelfHost;
        private int? port;
        private string serverAddress;

        public SettingsViewModel(LotteryTicketChecker lotteryTicketChecker)
        {
            this.lotteryTicketChecker = lotteryTicketChecker;

            this.settings = SettingsManager.Get();
            this.IsSelfHost = this.settings.IsSelfHost;
            this.Port = this.settings.Port;
            this.BackupDatabase = this.settings.BackupDatabase;
            this.BackupPath = this.settings.BackupPath;
            this.BackupHours = this.settings.BackupHours;
            this.BackupMinutes = this.settings.BackupMinutes;
            this.ServerAddress = this.settings.ServerAddress;
            this.ReceiptPath = this.settings.ReceiptPath;
            this.LotteryUsername = this.settings.LotteryUsername;
            this.LotteryPassword = this.settings.LotteryPassword;
            this.OpenHours = this.settings.OpenHours;
            this.OpenMinutes = this.settings.OpenMinutes;
            this.CloseHours = this.settings.CloseHours;
            this.CloseMinutes = this.settings.CloseMinutes;
            this.ShutdownIsChecked = this.settings.Shutdown;
            this.SleepIsChecked = this.settings.Sleep;
        }

        public ICommand SaveCommand => new RelayCommand(this.Save, this.CanSave);

        private bool CanSave()
        {
            return this.HasNoValidationErrors()
                   && (!this.BackupDatabase || Directory.Exists(this.BackupPath))
                   && Directory.Exists(this.ReceiptPath);
        }

        //public ICommand CancelCommand => new RelayCommand(() => ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowLogin());

        public ICommand BackupCommand => new RelayCommand(() => BackupDatabaseService.Backup(this.BackupPath), () => Directory.Exists(this.BackupPath));

        public ICommand LotteryLoginCommand => new RelayCommand(this.LotteryLogin);

        public bool IsSelfHost
        {
            get => this.isSelfHost;
            set
            {
                this.isSelfHost = value;
                if (value)
                {
                    this.ServerAddress = this.GetAddress();
                }
            }
        }

        [ValidateNullableInt(1, 65535)]
        public int? Port
        {
            get => this.port;
            set
            {
                this.port = value;
                if (this.IsSelfHost)
                {
                    this.ServerAddress = this.GetAddress();
                }
            }
        }

        public bool BackupDatabase { get; set; }

        public string BackupPath { get; set; }

        [ValidateNullableInt(0, 23)]
        public int? BackupHours { get; set; }

        [ValidateNullableInt(0, 59)]
        public int? BackupMinutes { get; set; }

        [ValidateUri]
        public string ServerAddress
        {
            get => this.serverAddress;
            set => this.Set(ref this.serverAddress, value);
        }

        public string ReceiptPath { get; set; }

        public string LotteryUsername { get; set; }

        public string LotteryPassword { get; set; }

        [ValidateNullableInt(0, 23)]
        public int? OpenHours { get; set; }

        [ValidateNullableInt(0, 59)]
        public int? OpenMinutes { get; set; }

        [ValidateNullableInt(0, 23)]
        public int? CloseHours { get; set; }

        [ValidateNullableInt(0, 59)]
        public int? CloseMinutes { get; set; }

        public bool ShutdownIsChecked { get; set; }

        public bool SleepIsChecked { get; set; }

        private string GetAddress()
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var selfHostAddress = $"http://{localIp}:{this.Port.GetValueOrDefault()}";
            return selfHostAddress;
        }

        private void Save()
        {
            this.settings.IsSelfHost = this.IsSelfHost;
            this.settings.Port = this.Port.Value;
            this.settings.BackupDatabase = this.BackupDatabase;
            this.settings.BackupPath = this.BackupPath;
            this.settings.BackupHours = this.BackupHours.Value;
            this.settings.BackupMinutes = this.BackupMinutes.Value;
            this.settings.ServerAddress = this.ServerAddress;
            this.settings.ReceiptPath = this.ReceiptPath;
            this.settings.LotteryUsername = this.LotteryUsername;
            this.settings.LotteryPassword = this.LotteryPassword;
            this.settings.OpenHours = this.OpenHours.Value;
            this.settings.OpenMinutes = this.OpenMinutes.Value;
            this.settings.CloseHours = this.CloseHours.Value;
            this.settings.CloseMinutes = this.CloseMinutes.Value;
            this.settings.Shutdown = this.ShutdownIsChecked;
            this.settings.Sleep = this.SleepIsChecked;
            SettingsManager.Save(this.settings);

            ViewModelLocator.Container.Resolve<MainWindowViewModel>().InitializeAsync(null);
        }

        //TODO: add better error messages
        private async void LotteryLogin()
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