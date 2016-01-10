namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using Autofac;

    using Common.Contracts;
    using Common.Helpers;

    using Properties;

    public class ViewModelLocator
    {
        private IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public ProductsEditorViewModel ProductsEditorViewModel => this.container.Resolve<ProductsEditorViewModel>();

        public ActiveServersViewModel ActiveServersViewModel => this.container.Resolve<ActiveServersViewModel>();

        public SettingsViewModel SettingsViewModel => this.container.Resolve<SettingsViewModel>();

        public DeliveryViewModel AddNewMainProductViewModel => this.container.Resolve<DeliveryViewModel>();

        private void Register()
        {
            if (Settings.Default.Servers == string.Empty)
            {
                Settings.Default.Servers = "[]";
                Settings.Default.Save();
            }

            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<ServerTester>().As<IServerTester>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SettingsStorage>().As<IServerStorage>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ServerManager>().As<IServerManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProductsCache>().As<IProductsCache>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<NameFromBarcodeGenerator>().As<INameFromBarcodeGenerator>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductsEditorViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActiveServersViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SettingsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DeliveryViewModel>().InstancePerLifetimeScope();

            this.container = containerBuilder.Build();
        }
    }
}