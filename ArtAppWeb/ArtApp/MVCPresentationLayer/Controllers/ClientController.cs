using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataObjects;
using LogicLayer;

namespace MVCPresentationLayer.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private IClientManager _clientManager = null;

        public ClientController()
        {
            _clientManager = new ClientManager();
        }


        // GET: Client
        public ActionResult Index()
        {
            ViewBag.Title = "Clients";
            var clients = _clientManager.RetrieveAllClients();
            return View(clients);
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Client Details";
            var client = _clientManager.GetClientByID(id);
            return View(client);
        }

        // GET: Client/Create
        [Authorize(Roles = "Customer Service, Manager")]
        public ActionResult Create()
        {
            ViewBag.Title = "Creating A New client";
            return View();
        }

        // POST: Client/Create
        [Authorize(Roles = "Customer Service, Manager")]
        [HttpPost]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    _clientManager.AddClient(client);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Client/Edit/5
        [Authorize(Roles = "Customer Service, Manager")]
        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Editing Client";

            var client = _clientManager.GetClientByID(id);

            return View(client);
        }

        // POST: Client/Edit/5
        [HttpPost]
        [Authorize(Roles = "Customer Service, Manager")]
        public ActionResult Edit(int id, Client newClient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldClient = _clientManager.GetClientByID(id);

                    // TODO: Add insert logic here
                    _clientManager.EditClient(oldClient, newClient);

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        /*
        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
