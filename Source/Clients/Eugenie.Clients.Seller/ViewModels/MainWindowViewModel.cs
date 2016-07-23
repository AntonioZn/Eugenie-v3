namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Notifications;

    using GalaSoft.MvvmLight;

    using Helpers;

    using Server.Host;

    using Views;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler, IBarcodeHandler
    {
        private readonly IWebApiHost webApiHost;
        private readonly SettingsManager settingsManager;
        private UserControl content;

        public MainWindowViewModel(IWebApiHost webApiHost, SettingsManager settingsManager)
        {
            this.webApiHost = webApiHost;
            this.settingsManager = settingsManager;

            TeamViewerPopupBlocker.Start();

            if (string.IsNullOrEmpty(this.settingsManager.Settings.Address))
            {
                this.Content = new Settings();
            }
            else
            {
                this.Content = new Login();
            }

            this.Initialize();
        }

        public UserControl Content
        {
            get
            {
                return this.content;
            }

            private set
            {
                this.Set(() => this.Content, ref this.content, value);
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            (this.Content.DataContext as IKeyHandler)?.HandleKey(e, key);
        }

        public void HandleBarcode(string barcode)
        {
            (this.Content.DataContext as IBarcodeHandler)?.HandleBarcode(barcode);
        }

        public void HostServer(int port)
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var address = "http://" + localIp + ":" + port;
            try
            {
                this.webApiHost.HostWebApi(address);
                NotificationsHost.Success("Успешно", "Сървърът беше стартиран успешно.");
            }
            catch (AccessDeniedException)
            {
                NotificationsHost.Error("Неуспешно стартиране на сървъра", "Програмата трябва да бъде стартирана като администратор.");
            }
            catch (PortInUseException)
            {
                NotificationsHost.Error("Неуспешно стартиране на сървъра", "Портът се използва от друга програма.");
            }
        }

        public void Initialize()
        {
            if (this.settingsManager.Settings.IsSelfHost)
            {
                this.HostServer(this.settingsManager.Settings.Port);
            }

            if (this.settingsManager.Settings.BackupDatabase)
            {
                var hours = this.settingsManager.Settings.BackupHours;
                var minutes = this.settingsManager.Settings.BackupMinutes;
                var path = this.settingsManager.Settings.BackupPath;
                this.webApiHost.AutoBackupDatabase(hours, minutes, path);
            }
        }

        public void ShowLogin()
        {
            this.Content = new Login();
        }

        public void ShowSell()
        {
            this.Content = new Sell();
        }

        public void ShowSettings()
        {
            this.Content = new Settings();
        }
    }
}