using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContestManagement.Web.Admin.Models;

namespace ContestManagement.Web.Admin.Controllers
{
	public class ContestController : Controller
	{
		public ActionResult Locate()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create([Bind(Exclude = "Id,AccessCode,Seats,WasEverPublished")] ContestInfoViewModel contest)
		{
			if (ModelState.IsValid)
			{
				//try
				//{
				//	contest.Id = GuidUtil.NewSequentialId();
				//	this.Service.CreateConference(conference);
				//}
				//catch (DuplicateNameException e)
				//{
				//	ModelState.AddModelError("Slug", e.Message);
				//	return View(conference);
				//}

				return RedirectToAction("Index", new { slug = contest.Slug, accessCode = contest.AccessCode });
			}

			return View(contest);
		}
	}
}