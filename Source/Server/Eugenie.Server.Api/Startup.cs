using Eugenie.Server.Api;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Eugenie.Server.Api
{
    using System.Web.Http;

    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DatabaseConfig.Initialize();
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            this.ConfigureAuth(app);
            app.UseWebApi(config);
        }
    }
}