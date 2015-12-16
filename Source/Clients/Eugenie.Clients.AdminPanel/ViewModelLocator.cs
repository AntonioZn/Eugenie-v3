namespace Eugenie.Clients.AdminPanel
{
    using Autofac;

    using GalaSoft.MvvmLight.Messaging;

    using ViewModels;

    public class ViewModelLocator
    {
        private IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public ProductsEditorViewModel ProductsEditorViewModel => this.container.Resolve<ProductsEditorViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<Messenger>().As<IMessenger>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProductsEditorViewModel>();

            this.container = containerBuilder.Build();
        }
    }
}