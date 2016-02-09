namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Notifications;

    using GalaSoft.MvvmLight;

    using Server.Host;

    using Squirrel;

    using Views;

    using Settings = Properties.Settings;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler
    {
        private readonly IWebApiHost webApiHost;
        private UserControl content;

        public MainWindowViewModel(IWebApiHost webApiHost)
        {
            this.webApiHost = webApiHost;
            if (string.IsNullOrEmpty(Settings.Default.Address))
            {
                this.Content = new Views.Settings();
            }
            else
            {
                this.Content = new Login();
            }

            Task.Run(async () =>
                           {
                               using (var mgr = new UpdateManager("https://s3.eu-central-1.amazonaws.com/eugenie95/seller/"))
                               {
                                   await mgr.UpdateApp();
                               }
                           });

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
            var keyHandler = this.Content.DataContext as IKeyHandler;
            if (keyHandler != null && keyHandler != this)
            {
                keyHandler.HandleKey(e, key);
            }
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
            if (Settings.Default.IsSelfHost)
            {
                this.HostServer(Settings.Default.Port);
            }

            if (Settings.Default.BackupDatabase)
            {
                var hours = Settings.Default.BackupHours;
                var minutes = Settings.Default.BackupMinutes;
                var path = Settings.Default.BackupPath;
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
            this.Content = new Views.Settings();
        }
    }
}