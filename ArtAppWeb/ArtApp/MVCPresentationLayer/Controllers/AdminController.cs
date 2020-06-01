using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using MVCPresentationLayer.Models;

namespace MVCPresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        // GET: Admin
        public ActionResult Index()
        {
            //return View(db.ApplicationUsers.ToList());
            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View(userManager.Users.OrderBy(n => n.DisplayName).ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ApplicationUser applicationUser = db.ApplicationUsers.Find(id);
            userManager = userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser applicationUser = userManager.FindById(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            // get a list of roles the user has and put them into a viewbad as 'roles'
            // along with a list of roles the user doesnt have as 'noRoles'
            var usrMgr = new LogicLayer.UserManager();
            var allRoles = usrMgr.RetrieveUserRoles();

            var roles = userManager.GetRoles(id);
            var noRoles = allRoles.Except(roles);

            ViewBag.Roles = roles;
            ViewBag.NoRoles = noRoles;

            return View(applicationUser);
        }


        public ActionResult RemoveRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            // code to prevent the removal of the last admin account
            if (role == "Admin")
            {
                var adminUsers = userManager.Users.ToList()
                    .Where(u => userManager.IsInRole(u.Id, "Admin"))
                    .ToList().Count();
                if (adminUsers < 2)
                {
                    ViewBag.Error = "Cannot remove last administrator.";
                    return RedirectToAction("Details", "Admin", new { id = user.Id });
                }
                //else
                //{
                //    userManager.RemoveFromRole(id, role);
                //}
            }
            userManager.RemoveFromRole(id, role);

            if (user.WorkerID != null)
            {
                try
                {
                    var usrMgr = new LogicLayer.UserManager();
                    usrMgr.DeleteUserRole((int)user.WorkerID, role);
                }
                catch (Exception)
                {
                    // Do nothing I guess.
                }
            }


            return RedirectToAction("Details", "Admin", new { id = user.Id });

        }
        public ActionResult AddRole(string id, string role)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.Users.First(u => u.Id == id);

            userManager.AddToRole(id, role);

            if (user.WorkerID != null)
            {
                try
                {
                    var usrMgr = new LogicLayer.UserManager();
                    usrMgr.AddUserRole((int)user.WorkerID, role);
                }
                catch (Exception)
                {
                    // Do nothing I guess.
                }
            }

            return RedirectToAction("Details", "Admin", new { id = user.Id });
        }

    }
}
