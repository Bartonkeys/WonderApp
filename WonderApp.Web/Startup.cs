using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WonderApp.Web.Startup))]
namespace WonderApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
