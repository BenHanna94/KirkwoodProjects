using LogicLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    [Authorize(Roles = "Manager, Contributor")]
    public class PieceController : Controller
    {
        private IPieceManager _pieceManager = null;
        private IUserManager _userManager = null;
        private IProjectManager _projectManager = null;
        private Project _project = null;

        // GET is what passes the page something on creation. POST is what the page returns.

        public PieceController()
        {
            _pieceManager = new PieceManager();
            _projectManager = new ProjectManager();
            _userManager = new UserManager();
        }

        public PieceController(Project project)
        {
            _pieceManager = new PieceManager();
            _projectManager = new ProjectManager();
            _userManager = new UserManager();
            _project = project;
        }

        /*
        // GET: Piece
        public ActionResult Index()
        {
            ViewBag.Title = "Pieces for Project " + _project.Name;
            var pieces = _pieceManager.GetPiecesByProject(_project.ProjectID);
            return View();
        }
        */


        // GET: Piece by Project
        [Authorize]
        public ActionResult Index(int id, bool complete = false)
        {
            ViewBag.Title = "Pieces for Project";
            var pieces = _pieceManager.GetPiecesByProject(id, complete);
            _project = _projectManager.GetProjectByID(id);
            return View(pieces);
        }

        // GET: Piece/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Piece Details";
            var piece = _pieceManager.GetPieceByID(id);

            return View(piece);
        }

        // GET: Piece/Create
        public ActionResult Create()
        {
            ViewBag.ProjectList = _projectManager.GetProjectsByComplete(false);
            ViewBag.UserList = _userManager.RetrieveUserListByActive(true);
            ViewBag.StatusList = _pieceManager.GetAllCompensatedStatuses();
            ViewBag.Title = "Add a new Piece";
            return View();
        }


        // POST: Piece/Create
        [HttpPost]
        public ActionResult Create(Piece piece)
        {
            ViewBag.ProjectList = _projectManager.GetProjectsByComplete(false);
            ViewBag.UserList = _userManager.RetrieveUserListByActive(true);
            ViewBag.StatusList = _pieceManager.GetAllCompensatedStatuses();

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    _pieceManager.AddPiece(piece);

                    return RedirectToAction("Index", new { id = piece.ProjectID });
                }
                catch
                {
                    return View();
                } 
            }
            return View();
        }

        // GET: Piece/Edit/5
        public ActionResult Edit(int id)
        {
            Piece piece = _pieceManager.GetPieceByID(id);
            ViewBag.ProjectList = _projectManager.GetProjectsByComplete(false);
            ViewBag.UserList = _userManager.RetrieveUserListByActive(true);
            ViewBag.StatusList = _pieceManager.GetAllCompensatedStatuses();
            ViewBag.Title = "Edit Piece";
            return View(piece);
        }

        // POST: Piece/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Piece newPiece)
        {
            ViewBag.ProjectList = _projectManager.GetProjectsByComplete(false);
            ViewBag.UserList = _userManager.RetrieveUserListByActive(true);
            ViewBag.StatusList = _pieceManager.GetAllCompensatedStatuses();

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    Piece oldPiece = _pieceManager.GetPieceByID(id);
                    _pieceManager.EditPiece(oldPiece, newPiece);

                    return RedirectToAction("Index", new { id = newPiece.ProjectID });
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        // GET: Piece/Delete/5
        public ActionResult Delete(int id)
        {
            Piece piece = null;
            try
            {
                piece = _pieceManager.GetPieceByID(id);
            }
            catch (Exception)
            {
                RedirectToAction("Index");
            }

            return View(piece);
        }

        // POST: Piece/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Piece piece)
        {
            piece = _pieceManager.GetPieceByID(id);

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add delete logic here
                    if (piece.Complete)
                    {
                        _pieceManager.SetPieceCompleteStatus(false, piece.PieceID);
                    }
                    else
                    {
                        _pieceManager.SetPieceCompleteStatus(true, piece.PieceID);
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }
    }
}
