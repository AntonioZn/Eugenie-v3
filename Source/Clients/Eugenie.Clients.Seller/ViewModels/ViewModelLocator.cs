namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Net.Http;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;

    using Helpers;

    using Server.Host;

    public class ViewModelLocator
    {
        public static HttpClient httpClient;

        public ViewModelLocator()
        {
            this.Register();
        }

        public static IContainer Container { get; private set; }

        public MainWindowViewModel MainWindowViewModel => Container.Resolve<MainWindowViewModel>();

        public LoginViewModel LoginViewModel => Container.Resolve<LoginViewModel>();

        public SellViewModel SellViewModel => Container.Resolve<SellViewModel>();

        public SettingsViewModel SettingsViewModel => Container.Resolve<SettingsViewModel>();

        public SettingsManager SettingsManager => Container.Resolve<SettingsManager>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().SingleInstance();
            containerBuilder.RegisterType<WebApiHost>().As<IWebApiHost>().SingleInstance();

            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>();
            containerBuilder.RegisterType<SellViewModel>();
            containerBuilder.RegisterType<SettingsViewModel>().SingleInstance();
            containerBuilder.RegisterType<SettingsManager>().SingleInstance();
            containerBuilder.RegisterType<FiscalPrinterHandler>().SingleInstance();

            Container = containerBuilder.Build();
        }
    }
}