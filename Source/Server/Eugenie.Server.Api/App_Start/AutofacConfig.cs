namespace Eugenie.Server.Api.App_Start
{
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Data;

    using Owin;

    using Services.Data.Contracts;

    public static class AutofacConfig
    {
        public static void RegisterAutofac(IAppBuilder app, HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            Register(builder);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        private static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<EugenieDbContext>().As<IEugenieDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof (EfGenericRepository<>)).As(typeof (IRepository<>)).InstancePerRequest();

            var servicesAssembly = Assembly.GetAssembly(typeof (IProductsService));
            builder.RegisterAssemblyTypes(servicesAssembly).AsImplementedInterfaces();

            //builder.RegisterType<ProductsService>().As<IProductsService>().InstancePerRequest();
            //builder.RegisterType<DealsService>().As<IDealsService>().InstancePerRequest();
            //builder.RegisterType<ReportsService>().As<IReportsService>().InstancePerRequest();
            //builder.RegisterType<MissingProductsService>().As<IMissingProductsService>().InstancePerRequest();
        }
    }
}