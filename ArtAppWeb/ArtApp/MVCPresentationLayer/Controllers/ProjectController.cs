using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    [Authorize(Roles = "Manager, Contributor")]
    public class ProjectController : Controller
    {
        private IProjectManager _projectManager = null;

        public ProjectController()
        {
            _projectManager = new ProjectManager();
        }

        // GET is what passes the page something on creation. POST is what the page returns.

        // GET: Project
        [Authorize]
        public ActionResult Index(bool complete = false)
        {
            ViewBag.Title = "Projects";
            var projects = _projectManager.GetProjectsByComplete(complete);
            return View(projects);
        }

        // GET: Project/Details/5
        [Authorize(Roles = "Manager, Contributor")]
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Project Details";
            var project = _projectManager.GetProjectByID(id);
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Adding a New Project";
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    _projectManager.AddProject(project);


                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }
            return View();
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            Project project = _projectManager.GetProjectByID(id);

            ViewBag.Title = "Editing Project";

            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Project newProject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here

                    Project oldProject = _projectManager.GetProjectByID(id);

                    _projectManager.EditProject(oldProject, newProject);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }
            return View();
        }

        // GET: Project/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            Project project = null;
            try
            {
                project = _projectManager.GetProjectByID(id);
            }
            catch(Exception)
            {
                RedirectToAction("Index");
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public ActionResult Delete(int id, Project project)
        {
            project = _projectManager.GetProjectByID(id);

            try
            {
                // TODO: Add delete logic here
                if (project.Complete)
                {
                    _projectManager.SetProjectCompleteStatus(false, project.ProjectID);
                }
                else
                {
                    _projectManager.SetProjectCompleteStatus(true, project.ProjectID);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
