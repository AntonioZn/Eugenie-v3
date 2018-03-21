﻿namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Helpers;

    using Helpers;

    using Server.Host;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Mvvm;

    using Views;

    using Settings = Models.Settings;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler, IBarcodeHandler
    {
        private readonly IWebApiHost webApiHost;
        private readonly Settings settings;
        private UserControl content;

        public MainWindowViewModel(IWebApiHost webApiHost, Settings settings)
        {
            this.webApiHost = webApiHost;
            this.settings = settings;

            TeamViewerPopupBlocker.Start();

            //if (string.IsNullOrEmpty(this.settings.Address))
           // {
           //     this.Content = new Views.Settings();
            //}
            //else
            //{
                this.Content = new Login();
            //}

            this.Initialize();
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

        public void HostServer(int port)
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var address = $"http://{localIp}:{port}";
            try
            {
                this.webApiHost.HostWebApi(address);
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

        public void Initialize()
        {
            if (this.settings.IsSelfHost)
            {
                this.HostServer(this.settings.Port);
            }

            if (this.settings.BackupDatabase)
            {
                var hours = this.settings.BackupHours;
                var minutes = this.settings.BackupMinutes;
                var path = this.settings.BackupPath;
                //this.webApiHost.AutoBackupDatabase(hours, minutes, path);
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