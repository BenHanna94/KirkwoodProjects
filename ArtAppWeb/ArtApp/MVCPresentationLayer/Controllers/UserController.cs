using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer;
using DataObjects;

namespace MVCPresentationLayer.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private IUserManager _userManager;

        public UserController()
        {
            _userManager = new UserManager();
        }

        // GET: User
        public ActionResult Index()
        {
            ViewBag.Title = "All Users";
            var users = _userManager.RetrieveUserListByActive();
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Title = "User Details";
            User user = _userManager.RetrieveUserByID(id);


            var userManager = new LogicLayer.UserManager();
            var allSkills = userManager.RetrieveUserSkills();

            //This overload gets skills by the user, not every single skill.
            var skills = userManager.RetrieveUserSkills(id);
            var noSkills = allSkills.Except(skills);

            ViewBag.Skills = skills;
            ViewBag.NoSkills = noSkills;


            return View(user);
        }

        // GET: User/Create
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            ViewBag.Title = "Creating a new User";
            ViewBag.Roles = _userManager.RetrieveUserRoles();
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    _userManager.AddUser(user);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }
            return View();
        }

        // GET: User/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int id)
        {
            User user = _userManager.RetrieveUserByID(id);

            ViewBag.Roles = _userManager.RetrieveUserRoles();
            ViewBag.Title = "Editing User";

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int id, User newUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    User oldUser = _userManager.RetrieveUserByID(id);
                    _userManager.EditUser(oldUser, newUser);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }
            return View();
        }

        // GET: User/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(int id)
        {
            User user = null;

            try
            {
                user = _userManager.RetrieveUserByID(id);
            }
            catch (Exception)
            {
                RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: User/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult Delete(int id, User user)
        {
            try
            {
                // TODO: Add delete logic here
                user = _userManager.RetrieveUserByID(id);

                if (user.Active)
                {
                    _userManager.SetUserActiveState(false, id); 
                }
                else
                {
                    _userManager.SetUserActiveState(true, id);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult RemoveSkill(int id, string skill)
        {
            var userManager = new LogicLayer.UserManager();
            var user = userManager.RetrieveUserByID(id);

            // code to prevent the removal of the last admin account
            userManager.DeleteUserSkill(id, skill);


            return RedirectToAction("Details", "User", new { id = user.UserID });

        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AddSkill(int id, string skill)
        {
            var userManager = new LogicLayer.UserManager();
            var user = userManager.RetrieveUserByID(id);

            userManager.AddUserSkill(id, skill);


            return RedirectToAction("Details", "User", new { id = user.UserID });
        }

    }
}
