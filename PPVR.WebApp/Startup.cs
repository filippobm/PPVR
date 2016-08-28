using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PPVR.WebApp.Startup))]
namespace PPVR.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
