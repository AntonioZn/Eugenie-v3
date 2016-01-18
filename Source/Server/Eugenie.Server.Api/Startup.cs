using Eugenie.Server.Api;

using Microsoft.Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Eugenie.Server.Api
{
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Data;

    using Owin;

    using Services.Data;
    using Services.Data.Contracts;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DatabaseConfig.Initialize();

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            this.ConfigureAuth(app);

            var builder = new ContainerBuilder();
            this.Register(builder);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        private void Register(ContainerBuilder builder)
        {
            builder.RegisterType<EugenieDbContext>().As<IEugenieDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfGenericRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<ProductsService>().As<IProductsService>();
            builder.RegisterType<DealsService>().As<IDealsService>();
            builder.RegisterType<ReportsService>().As<IReportsService>();
            builder.RegisterType<MissingProductsService>().As<IMissingProductsService>();
        }
    }
}