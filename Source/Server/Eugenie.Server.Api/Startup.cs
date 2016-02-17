using Eugenie.Server.Api;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Eugenie.Server.Api
{
    using System.Web.Http;

    using App_Start;

    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DatabaseConfig.Initialize();

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            this.ConfigureAuth(app);
            AutofacConfig.RegisterAutofac(app, config);
        }
    }
}