namespace Eugenie.Clients.Seller.ViewModels
{
    using Autofac;

    using Common;

    using Helpers;

    using Server.Host;

    using Sv.Wpf.Core.Helpers;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            this.Register();
        }

        public static IContainer Container { get; private set; }

        public INavigationService NavigationService => Container.Resolve<INavigationService>();

        public MainWindowViewModel MainWindowViewModel => Container.Resolve<MainWindowViewModel>();

        public LoginViewModel LoginViewModel => Container.Resolve<LoginViewModel>();

        public SettingsViewModel SettingsViewModel => Container.Resolve<SettingsViewModel>();

        public SellViewModel SellViewModel => Container.Resolve<SellViewModel>();

        public ProductsSearchViewModel ProductsSearchViewModel => Container.Resolve<ProductsSearchViewModel>();

        public LotteryTicketCheckerViewModel LotteryTicketCheckerViewModel => Container.Resolve<LotteryTicketCheckerViewModel>();

        public ChangeCalculatorViewModel ChangeCalculatorViewModel => Container.Resolve<ChangeCalculatorViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiHost>().As<IWebApiHost>().SingleInstance();
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            containerBuilder.RegisterType<TaskManager>().SingleInstance();

            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>();
            containerBuilder.RegisterType<SettingsViewModel>();
            containerBuilder.RegisterType<SellViewModel>();
            containerBuilder.RegisterType<ProductsSearchViewModel>();
            containerBuilder.RegisterType<LotteryTicketCheckerViewModel>();
            containerBuilder.RegisterType<ChangeCalculatorViewModel>();

            containerBuilder.RegisterType<FiscalPrinterHandler>().SingleInstance();
            containerBuilder.RegisterType<LotteryTicketChecker>().SingleInstance();
            
            Container = containerBuilder.Build();
        }
    }
}