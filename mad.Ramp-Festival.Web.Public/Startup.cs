using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mad.Ramp_Festival.Web.Public.Startup))]
namespace mad.Ramp_Festival.Web.Public
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
