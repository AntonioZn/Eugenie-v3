﻿namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Notifications;

    using GalaSoft.MvvmLight;

    using Server.Host;

    using Views;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler
    {
        private readonly IWebApiHost webApiHost;
        private UserControl content;

        public MainWindowViewModel(IWebApiHost webApiHost)
        {
            this.webApiHost = webApiHost;
            if (string.IsNullOrEmpty(Properties.Settings.Default.Address))
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

            set
            {
                this.Set(() => this.Content, ref this.content, value);
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

        public void HandleKey(KeyEventArgs e, Key key)
        {
            (this.content.DataContext as IKeyHandler)?.HandleKey(e, key);
        }

        public void Initialize()
        {
            if (Properties.Settings.Default.IsSelfHost)
            {
                this.HostServer(Properties.Settings.Default.Port);
            }

            if (Properties.Settings.Default.BackupDatabase)
            {
                var hours = Properties.Settings.Default.BackupHours;
                var minutes = Properties.Settings.Default.BackupMinutes;
                var path = Properties.Settings.Default.BackupPath;
                this.webApiHost.AutoBackupDatabase(hours, minutes, path);
            }
        }
    }
}