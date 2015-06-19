using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Controllers
{
    public class HomeController : Controller
    {
        protected const string TryCounterName = "TryCounterName";
        private IUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        public HomeController(IUnitOfWork uow, IWebLogger logger)
        {
            Database = uow;
            Log = logger;
        }
        public ActionResult Index()
        {
            return View(new DocumentCredentials());
        }

        public ActionResult GetDocument(DocumentCredentials doc)
        {
            var tryCounter = 0;
            if (Session[TryCounterName] != null)
            {
                tryCounter = (int)Session[TryCounterName];
            }
            tryCounter += 1;
            if (ModelState.IsValid)
            {
                var found = Database.ServiceDocuments.Find((document) => document.PublishId == doc.PublishId).FirstOrDefault();
                if (found == null)
                {
                    ModelState.AddModelError("PublishId", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }
                else if (found.PublishPassword != doc.PublishPassword)
                {
                    ModelState.AddModelError("PublishId", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }
                else
                {
                    Session[TryCounterName] = 0;
                    return RedirectToAction("Index");
                }
                
            }
            Session[TryCounterName] = tryCounter;
            return View("Index");
        }
    }
}