using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mad.Ramp_Festival.Web.Public.Models;

namespace mad.Ramp_Festival.Web.Public.Areas.Admin.Controllers
{
	public class ApplicationUsersController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Admin/ApplicationUsers
		public async Task<ActionResult> Index()
		{
			return View(await db.Users.ToListAsync());
		}

		// GET: Admin/ApplicationUsers/Details/5
		public async Task<ActionResult> Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		// GET: Admin/ApplicationUsers/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Admin/ApplicationUsers/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Id,DisplayName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
		{
			if (ModelState.IsValid)
			{
				db.Users.Add(applicationUser);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(applicationUser);
		}

		// GET: Admin/ApplicationUsers/Edit/5
		public async Task<ActionResult> Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		// POST: Admin/ApplicationUsers/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Id,DisplayName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
		{
			if (ModelState.IsValid)
			{
				db.Entry(applicationUser).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(applicationUser);
		}

		// GET: Admin/ApplicationUsers/Delete/5
		public async Task<ActionResult> Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		// POST: Admin/ApplicationUsers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(string id)
		{
			ApplicationUser applicationUser = await db.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));
			db.Users.Remove(applicationUser);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
