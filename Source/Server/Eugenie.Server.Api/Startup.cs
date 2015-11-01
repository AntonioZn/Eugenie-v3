using Eugenie.Server.Api;

using Microsoft.Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Eugenie.Server.Api
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DatabaseConfig.Initialize();
            this.ConfigureAuth(app);
        }
    }
}