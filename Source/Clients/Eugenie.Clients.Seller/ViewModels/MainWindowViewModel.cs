namespace Eugenie.Clients.Seller.ViewModels
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using Common.Notifications;

    using GalaSoft.MvvmLight;

    using Microsoft.Owin.Hosting;

    using Server.Api;

    using Views;

    public class MainWindowViewModel : ViewModelBase
    {
        private object content;

        public MainWindowViewModel()
        {
            if (Properties.Settings.Default.IsSelfHost)
            {
                HostServer(Properties.Settings.Default.Port);
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.Address))
            {
                this.Content = new Settings();
            }
            else
            {
                this.Content = new Login();
            }
        }

        public object Content
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

        public static void HostServer(int port)
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var selfHostAddress = "http://" + localIp + ":" + port;
            try
            {
                WebApp.Start<Startup>(selfHostAddress);
                NotificationsHost.Success("Успешно", "Сървърът беше стартиран успешно.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().Contains("denied"))
                {
                    NotificationsHost.Error("Неуспешно", "Програмата трябва да бъде стартирана като администратор.");
                }
                else
                {
                    NotificationsHost.Error("Неуспешно", "Портът се използва от друга програма.");
                }
            }
        }
    }
}