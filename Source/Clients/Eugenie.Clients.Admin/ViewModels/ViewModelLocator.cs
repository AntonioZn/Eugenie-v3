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

            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<Storage>().As<IServerStorage>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<Storage>().As<ITasksStorage>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ServerManager>().As<IServerManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<TaskManager>().As<ITaskManager>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductsEditorViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<MainWindowViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SettingsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DeliveryViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<MissingProductsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PaletteSelectorViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ReportsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SellersViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LowQuantityProductsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ExpiringProductsViewModel>().InstancePerLifetimeScope();

            container = containerBuilder.Build();
        }
    }
}