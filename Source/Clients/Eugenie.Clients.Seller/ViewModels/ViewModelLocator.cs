namespace Eugenie.Clients.Seller.ViewModels
{
    using Autofac;

    using Helpers;

    using Models;

    using Server.Host;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Helpers;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            this.Register();
        }

        public static IContainer Container { get; private set; }

        public MainWindowViewModel MainWindowViewModel => Container.Resolve<MainWindowViewModel>();

        public LoginViewModel LoginViewModel => Container.Resolve<LoginViewModel>();

        public SellViewModel SellViewModel => Container.Resolve<SellViewModel>();

        public SettingsViewModel SettingsViewModel => Container.Resolve<SettingsViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiHost>().As<IWebApiHost>().SingleInstance();

            containerBuilder.RegisterType<TaskManager>().SingleInstance();

            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            containerBuilder.RegisterType<SettingsViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>();
            containerBuilder.RegisterType<SellViewModel>();

            containerBuilder.RegisterType<FiscalPrinterHandler>().SingleInstance();
            containerBuilder.RegisterType<LotteryTicketChecker>().SingleInstance();
            
            Container = containerBuilder.Build();
        }
    }
}