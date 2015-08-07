using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MassTransit;

namespace ContestManagement.Web.Admin
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static IServiceBus EventBus { get; private set; }

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// Create the event bus
			try
			{
				MvcApplication.EventBus = ServiceBusFactory.New(sbc =>
				{
					sbc.UseJsonSerializer();

					sbc.UseMsmq(msmq =>
					{
						msmq.UseMulticastSubscriptionClient();
						msmq.VerifyMsmqConfiguration();
					});
					sbc.ReceiveFrom("msmq://pc-mad/ramp-festival_contestmanagement");
					sbc.SetNetwork("WORKGROUP");
				});
			}
			catch (Exception ex)
			{

			}
		}
	}
}
