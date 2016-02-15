namespace Eugenie.Clients.Admin.ViewModels
{
    using Autofac;

    using Common.Contracts;
    using Common.Helpers;

    using Contracts;

    using Helpers;

    using Properties;

    public class ViewModelLocator
    {
        public static IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public ProductsEditorViewModel ProductsEditorViewModel => container.Resolve<ProductsEditorViewModel>();

        public MainWindowViewModel MainWindowViewModel => container.Resolve<MainWindowViewModel>();

        public SettingsViewModel SettingsViewModel => container.Resolve<SettingsViewModel>();

        public DeliveryViewModel DeliveryViewModel => container.Resolve<DeliveryViewModel>();

        public MissingProductsViewModel MissingProductsViewModel => container.Resolve<MissingProductsViewModel>();

        public PaletteSelectorViewModel PaletteSelectorViewModel => container.Resolve<PaletteSelectorViewModel>();

        public ReportsViewModel ReportsViewModel => container.Resolve<ReportsViewModel>();

        public SellersViewModel SellersViewModel => container.Resolve<SellersViewModel>();

        public LowQuantityProductsViewModel LowQuantityProductsViewModel => container.Resolve<LowQuantityProductsViewModel>();

        public ExpiringProductsViewModel ExpiringProductsViewModel => container.Resolve<ExpiringProductsViewModel>();

        private void Register()
        {
            if (Settings.Default.Servers == string.Empty)
            {
                Settings.Default.Servers = "[]";
                Settings.Default.Save();
            }

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().SingleInstance();
            containerBuilder.RegisterType<Storage>().As<IServerStorage>().SingleInstance();
            containerBuilder.RegisterType<Storage>().As<ITasksStorage>().SingleInstance();
            containerBuilder.RegisterType<ServerManager>().As<IServerManager>().SingleInstance();
            containerBuilder.RegisterType<TaskManager>().As<ITaskManager>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductsEditorViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            containerBuilder.RegisterType<SettingsViewModel>().SingleInstance();
            containerBuilder.RegisterType<DeliveryViewModel>().SingleInstance();
            containerBuilder.RegisterType<MissingProductsViewModel>().SingleInstance();
            containerBuilder.RegisterType<PaletteSelectorViewModel>().SingleInstance();
            containerBuilder.RegisterType<ReportsViewModel>().SingleInstance();
            containerBuilder.RegisterType<SellersViewModel>().SingleInstance();
            containerBuilder.RegisterType<LowQuantityProductsViewModel>().SingleInstance();
            containerBuilder.RegisterType<ExpiringProductsViewModel>().SingleInstance();

            container = containerBuilder.Build();
        }
    }
}