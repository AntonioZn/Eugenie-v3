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

        private void Register()
        {
            if (Settings.Default.Servers == string.Empty)
            {
                Settings.Default.Servers = "[]";
                Settings.Default.Save();
            }

            var containerBuilder = new ContainerBuilder();

            var serversManager = new ServersManager();
            var client = new WebApiServerClient(serversManager);

            containerBuilder.RegisterInstance(serversManager).As<IServersManager>();
            containerBuilder.RegisterInstance(client).As<IWebApiServerClient>();
            containerBuilder.RegisterType<ProductsEditorViewModel>();
            containerBuilder.RegisterType<ActiveServersViewModel>();
            containerBuilder.RegisterType<SettingsViewModel>();

            this.container = containerBuilder.Build();
        }
    }
}