using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContestManagement.Web.Admin.Startup))]
namespace ContestManagement.Web.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
