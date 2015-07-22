using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContestManagement.Web.Admin
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Contest.Create",
				url: "create",
				defaults: new { controller = "Contest", action = "Create" }
			);

			routes.MapRoute(
				name: "Contest",
				url: "{slug}/{accessCode}/{action}",
				defaults: new { controller = "Contest", action = "Index" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
