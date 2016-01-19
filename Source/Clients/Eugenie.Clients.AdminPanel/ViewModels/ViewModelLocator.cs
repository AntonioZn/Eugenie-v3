﻿namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using Autofac;

    using Common.Contracts;
    using Common.Helpers;

    using Properties;

    public class ViewModelLocator
    {
        public static IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public ProductsEditorViewModel ProductsEditorViewModel => container.Resolve<ProductsEditorViewModel>();

        public ActiveServersViewModel ActiveServersViewModel => container.Resolve<ActiveServersViewModel>();

        public SettingsViewModel SettingsViewModel => container.Resolve<SettingsViewModel>();

        public DeliveryViewModel DeliveryViewModel => container.Resolve<DeliveryViewModel>();

        public MissingProductsViewModel MissingProductsViewModel => container.Resolve<MissingProductsViewModel>();

        public PaletteSelectorViewModel PaletteSelectorViewModel => container.Resolve<PaletteSelectorViewModel>();

        public ReportsViewModel ReportsViewModel => container.Resolve<ReportsViewModel>();

        private void Register()
        {
            if (Settings.Default.Servers == string.Empty)
            {
                Settings.Default.Servers = "[]";
                Settings.Default.Save();
            }

            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<WebApiClient>().As<IWebApiClient>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SettingsStorage>().As<IServerStorage>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ServerManager>().As<IServerManager>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductsEditorViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ActiveServersViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<SettingsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DeliveryViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<MissingProductsViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PaletteSelectorViewModel>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ReportsViewModel>().InstancePerLifetimeScope();

            container = containerBuilder.Build();
        }
    }
}