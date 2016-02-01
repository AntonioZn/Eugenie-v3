namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Net.Http;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;

    public class ViewModelLocator
    {
        public static IContainer container;
        public static HttpClient httpClient;

        public ViewModelLocator()
        {
            this.Register();
        }

        public MainWindowViewModel MainWindowViewModel => container.Resolve<MainWindowViewModel>();

        public LoginViewModel LoginViewModel => container.Resolve<LoginViewModel>();

        public SellViewModel SellViewModel => container.Resolve<SellViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<MainWindowViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<LoginViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SellViewModel>().InstancePerLifetimeScope();

            container = containerBuilder.Build();
        }
    }
}