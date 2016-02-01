namespace Eugenie.Clients.Seller.ViewModels
{
    using Autofac;

    using Common.Contracts;
    using Common.Helpers;

    public class ViewModelLocator
    {
        public static IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public MainWindowViewModel MainWindowViewModel => container.Resolve<MainWindowViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ServerManager>().As<IServerManager>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<MainWindowViewModel>().InstancePerLifetimeScope();

            container = containerBuilder.Build();
        }
    }
}