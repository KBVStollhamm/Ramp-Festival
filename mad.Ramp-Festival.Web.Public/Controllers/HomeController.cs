using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mad.Ramp_Festival.Web.Public.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult FestivalWeek()
		{
			return View();
		}

		public ActionResult Contests()
		{
			return View();
		}
	
		public ActionResult DailyGames()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Verantwortlich für den Inhalt dieser Seiten:";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}