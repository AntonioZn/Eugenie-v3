namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;

    public class ViewModelLocator
    {
        private IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public ProductsEditorViewModel ProductsEditorViewModel => this.container.Resolve<ProductsEditorViewModel>();

        public ActiveServersViewModel ActiveServersViewModel => this.container.Resolve<ActiveServersViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();
            var servers = new List<ServerInformation>
                          {
                              new ServerInformation("Eugenie", "Eugenie", "test 1", "http://localhost:9000"),
                              new ServerInformation("Eugenie", "Eugenie", "test 2", "http://localhost:9000")
                          };

            var serversManager = new ServersManager(servers);
            var client = new WebApiServerClient(serversManager);

            containerBuilder.RegisterInstance(serversManager).As<IServersManager>();
            containerBuilder.RegisterInstance(client).As<IWebApiServerClient>();
            containerBuilder.RegisterType<ProductsEditorViewModel>();
            containerBuilder.RegisterType<ActiveServersViewModel>();

            this.container = containerBuilder.Build();
        }
    }
}