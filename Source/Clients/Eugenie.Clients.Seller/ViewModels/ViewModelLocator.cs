namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Net.Http;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Notifications;

    using Helpers;

    using Models;

    using Server.Host;

    public class ViewModelLocator
    {
        public static HttpClient httpClient;

        public ViewModelLocator()
        {
            this.Register();
            
            new TeamViewerPopupBlocker().Start();
            this.LotteryTicketCheckerLogin();
        }

        public static IContainer Container { get; private set; }

        public MainWindowViewModel MainWindowViewModel => Container.Resolve<MainWindowViewModel>();

        public LoginViewModel LoginViewModel => Container.Resolve<LoginViewModel>();

        public SellViewModel SellViewModel => Container.Resolve<SellViewModel>();

        public SettingsViewModel SettingsViewModel => Container.Resolve<SettingsViewModel>();

        public Settings Settings => Container.Resolve<Settings>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().SingleInstance();
            containerBuilder.RegisterType<WebApiHost>().As<IWebApiHost>().SingleInstance();

            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>();
            containerBuilder.RegisterType<SellViewModel>();
            containerBuilder.RegisterType<SettingsViewModel>().SingleInstance();
            containerBuilder.RegisterType<FiscalPrinterHandler>().SingleInstance();
            containerBuilder.RegisterType<LotteryTicketChecker>().SingleInstance();

            var settings = new Settings();
            settings.Load();
            containerBuilder.RegisterInstance(settings).SingleInstance();

            Container = containerBuilder.Build();
        }

        private async void LotteryTicketCheckerLogin()
        {
            var settings = Container.Resolve<Settings>();
            if (!string.IsNullOrWhiteSpace(settings.LotteryUsername) && !string.IsNullOrWhiteSpace(settings.LotteryPassword))
            {
                var checker = Container.Resolve<LotteryTicketChecker>();
                var result = await checker.Login(settings.LotteryUsername, settings.LotteryPassword);
                if (result)
                {
                    NotificationsHost.Success("Успешно", "Успешен вход в националната лотария");
                }
                else
                {
                    NotificationsHost.Error("Грешка", "Грешно име или парола за националната лотария");
                }
            }
        }
    }
}