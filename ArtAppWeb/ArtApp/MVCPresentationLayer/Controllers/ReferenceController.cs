using DataObjects;
using LogicLayer;
using System;
using System.Web.Mvc;

namespace MVCPresentationLayer.Controllers
{
    [Authorize(Roles = "Manager, Contributor")]
    public class ReferenceController : Controller
    {
        private IReferenceManager _referenceManager = null;
        private IClientManager _clientManager = null;

        public ReferenceController()
        {
            _referenceManager = new ReferenceManager();
            _clientManager = new ClientManager();
        }

        // GET: Reference
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Current References";
            var references = _referenceManager.GetAllReferences();
            return View(references);
        }

        // GET: Reference/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Reference Details";
            var reference = _referenceManager.GetReferenceByID(id);

            return View(reference);
        }

        // GET: Reference/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Add A New Reference";
            ViewBag.ClientList = _clientManager.RetrieveAllClients();
            return View();
        }

        // POST: Reference/Create
        [HttpPost]
        public ActionResult Create(Reference reference)
        {
            ViewBag.ClientList = _clientManager.RetrieveAllClients();
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    _referenceManager.AddReference(reference);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                } 
            }
            return View();
        }

        // GET: Reference/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.ClientList = _clientManager.RetrieveAllClients();
            Reference reference = _referenceManager.GetReferenceByID(id);

            ViewBag.Title = "Edit Reference";
            return View(reference);
        }

        // POST: Reference/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Reference reference)
        {
            ViewBag.ClientList = _clientManager.RetrieveAllClients();
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    Reference oldReference = _referenceManager.GetReferenceByID(id);
                    _referenceManager.EditReference(oldReference, reference);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Reference/Delete/5
        public ActionResult Delete(int id)
        {
            Reference reference = null;
            try
            {
                reference = _referenceManager.GetReferenceByID(id);
            }
            catch (Exception)
            {
                RedirectToAction("index");
            }
            return View(reference);
        }

        // POST: Reference/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Reference reference)
        {
            try
            {
                // TODO: Add delete logic here
                _referenceManager.DeleteReference(reference);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
