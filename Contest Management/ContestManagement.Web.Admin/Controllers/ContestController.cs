using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContestManagement.Web.Admin.Models;

namespace ContestManagement.Web.Admin.Controllers
{
	public class ContestController : Controller
	{
		private ContestService service;
		private ContestService Service
		{
			get { return service ?? (service = new ContestService(MvcApplication.EventBus)); }
		}

		public ContestInfo Contest { get; private set; }

		/// <summary>
		/// We receive the slug value as a kind of cross-cutting value that 
		/// all methods need and use, so we catch and load the conference here, 
		/// so it's available for all. Each method doesn't need the slug parameter.
		/// </summary>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var slug = (string)this.ControllerContext.RequestContext.RouteData.Values["slug"];
			if (!string.IsNullOrEmpty(slug))
			{
				this.ViewBag.Slug = slug;
				this.Contest = this.Service.FindContest(slug);

				if (this.Contest != null)
				{
					// check access
					var accessCode = (string)this.ControllerContext.RequestContext.RouteData.Values["accessCode"];

					if (accessCode == null || !string.Equals(accessCode, this.Contest.AccessCode, StringComparison.Ordinal))
					{
						filterContext.Result = new HttpUnauthorizedResult("Invalid access code.");
					}
					else
					{
						this.ViewBag.OwnerName = this.Contest.OwnerName;
						this.ViewBag.WasEverPublished = this.Contest.WasEverPublished;
					}
				}
			}

			base.OnActionExecuting(filterContext);
		}

		public ActionResult Locate()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create([Bind(Exclude = "Id,AccessCode,Seats,WasEverPublished")] ContestInfo contest)
		{
			if (ModelState.IsValid)
			{
				try
				{
					contest.Id = GuidUtil.NewSequentialId();
					this.Service.CreateContest(contest);
				}
				catch (DuplicateNameException e)
				{
					ModelState.AddModelError("Slug", e.Message);
					return View(contest);
				}

				return RedirectToAction("Index", new { slug = contest.Slug, accessCode = contest.AccessCode });
			}

			return View(contest);
		}

		public ActionResult Index()
		{
			if (this.Contest == null)
			{
				return HttpNotFound();
			}
			return View(this.Contest);
		}
	}
}