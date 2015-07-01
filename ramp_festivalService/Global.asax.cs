using System.Web.Http;
using System.Web.Routing;

namespace ramp_festivalService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register();
        }
    }
}