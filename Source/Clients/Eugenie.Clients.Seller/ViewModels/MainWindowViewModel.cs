namespace Eugenie.Clients.Seller.ViewModels
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Helpers;

    using Helpers;

    using Server.Host;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Mvvm;

    using LotteryTicketChecker = Helpers.LotteryTicketChecker;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler, IBarcodeHandler
    {
        private readonly IWebApiHost webApiHost;
        private readonly LotteryTicketChecker lotteryTicketChecker;
        
        private UserControl content;
        private IDisposable host;

        public MainWindowViewModel(IWebApiHost webApiHost, LotteryTicketChecker lotteryTicketChecker)
        {
            this.webApiHost = webApiHost;
            this.lotteryTicketChecker = lotteryTicketChecker;

            TeamViewerPopupBlocker.Start();
        }

        public UserControl Content
        {
            get => this.content;
            private set => this.Set(ref this.content, value);
        }

        public StoreClient Client { get; set; }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            (this.Content.DataContext as IKeyHandler)?.HandleKey(e, key);
        }

        public void HandleBarcode(string barcode)
        {
            (this.Content.DataContext as IBarcodeHandler)?.HandleBarcode(barcode);
        }

        public override Task InitializeAsync(object navigationData)
        {
            this.host?.Dispose();

            var settings = SettingsManager.Get();
            if (string.IsNullOrEmpty(settings.ServerAddress))
            {
                this.Content = new Views.Settings();
                return Task.CompletedTask;
            }

            this.Content = new Views.Login();

            if (settings.IsSelfHost)
            {
                this.HostServer(settings.Port);
            }

            if (settings.BackupDatabase)
            {
                var hours = settings.BackupHours;
                var minutes = settings.BackupMinutes;
                var path = settings.BackupPath;
                //this.webApiHost.AutoBackupDatabase(hours, minutes, path);
            }

            return Task.CompletedTask;
        }

        public void ShowLogin()
        {
            this.Client?.Dispose();
            this.Content = new Views.Login();
        }

        public void ShowSell()
        {
            this.Content = new Views.Sell();
        }

        public void ShowSettings()
        {
            this.Content = new Views.Settings();
        }

        private void HostServer(int port)
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var address = $"http://{localIp}:{port}";
            try
            {
                this.host = this.webApiHost.HostWebApi(address);
                NotificationsHost.Success("Notifications", "Успешно", "Сървърът беше стартиран успешно.");
            }
            catch (AccessDeniedException)
            {
                NotificationsHost.Error("Notifications", "Неуспешно стартиране на сървъра", "Програмата трябва да бъде стартирана като администратор.");
            }
            catch (PortInUseException)
            {
                NotificationsHost.Error("Notifications", "Неуспешно стартиране на сървъра", "Портът се използва от друга програма.");
            }
        }
    }
}