namespace Eugenie.Clients.Seller.ViewModels
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Common;
    using Common.Contracts;

    using Helpers;

    using Models;

    using Server.Host;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Helpers;
    using Sv.Wpf.Core.Mvvm;

    using LotteryTicketChecker = Helpers.LotteryTicketChecker;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler, IBarcodeHandler
    {
        private readonly IWebApiHost webApiHost;
        private readonly INavigationService navigationService;
        private readonly LotteryTicketChecker lotteryChecker;

        private IDisposable host;

        public MainWindowViewModel(IWebApiHost webApiHost, INavigationService navigationService, TaskManager taskManager, LotteryTicketChecker lotteryChecker)
        {
            this.TaskManager = taskManager;
            this.webApiHost = webApiHost;
            this.navigationService = navigationService;
            this.lotteryChecker = lotteryChecker;

            TeamViewerPopupBlocker.Start();
            new WorkingTimeManager().Start();
        }

        public TaskManager TaskManager { get; }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            (this.navigationService.Content.DataContext as IKeyHandler)?.HandleKey(e, key);
        }

        public void HandleBarcode(string barcode)
        {
            (this.navigationService.Content.DataContext as IBarcodeHandler)?.HandleBarcode(barcode);
        }

        public override async Task InitializeAsync(object navigationData)
        {
            this.host?.Dispose();

            var settings = SettingsManager.Get();
            this.LotteryLogin(settings);
            if (string.IsNullOrEmpty(settings.ServerAddress))
            {
                await this.navigationService.NavigateToAsync<SettingsViewModel>();
            }

            await this.navigationService.NavigateToAsync<LoginViewModel>();

            if (settings.IsSelfHost)
            {
                this.HostServer(settings.Port);
            }

            if (settings.BackupDatabase)
            {
                var hours = settings.BackupHours;
                var minutes = settings.BackupMinutes;
                var path = settings.BackupPath;

                this.webApiHost.AutoBackupDatabase(hours, minutes, path);
            }
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

        private async void LotteryLogin(Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.LotteryUsername) && !string.IsNullOrWhiteSpace(settings.LotteryPassword))
            {
                try
                {
                    var result = await this.lotteryChecker.LoginAsync(settings.LotteryUsername, settings.LotteryPassword, CancellationToken.None);
                    if (result)
                    {
                        NotificationsHost.Success("Notifications", "Успешно", "Успешен вход в националната лотария");
                    }
                    else
                    {
                        NotificationsHost.Error("Notifications", "Грешка", "Грешно име или парола за националната лотария");
                    }
                }
                catch (Exception ex)
                {
                    NotificationsHost.Error("Notifications", "Грешка (нац. лотария)", ex.Message);
                }
            }
        }
    }
}