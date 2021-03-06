﻿namespace Eugenie.Clients.Admin.ViewModels
{
    using Autofac;

    using Helpers;

    using Properties;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            this.Register();
        }

        public static IContainer Container { get; private set; }

        public ProductsEditorViewModel ProductsEditorViewModel => Container.Resolve<ProductsEditorViewModel>();

        public MainWindowViewModel MainWindowViewModel => Container.Resolve<MainWindowViewModel>();

        public SettingsViewModel SettingsViewModel => Container.Resolve<SettingsViewModel>();

        public DeliveryViewModel DeliveryViewModel => Container.Resolve<DeliveryViewModel>();

        public MissingProductsViewModel MissingProductsViewModel => Container.Resolve<MissingProductsViewModel>();

        public PaletteSelectorViewModel PaletteSelectorViewModel => Container.Resolve<PaletteSelectorViewModel>();

        public ReportsViewModel ReportsViewModel => Container.Resolve<ReportsViewModel>();

        public SellersViewModel SellersViewModel => Container.Resolve<SellersViewModel>();

        public LowQuantityProductsViewModel LowQuantityProductsViewModel => Container.Resolve<LowQuantityProductsViewModel>();

        public ExpiringProductsViewModel ExpiringProductsViewModel => Container.Resolve<ExpiringProductsViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<ServerManager>().SingleInstance();
            containerBuilder.RegisterType<TaskManager>().SingleInstance();

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

            containerBuilder.RegisterInstance(Settings.Default);

            Container = containerBuilder.Build();
        }
    }
}