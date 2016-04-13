using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FoJiao.Startup))]

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace FoJiao
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
