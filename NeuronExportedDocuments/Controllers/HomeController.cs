using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
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

        public ActionResult GetDocument(IUserData userData, DocumentCredentials doc)
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
                    ModelState.AddModelError("", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }
                else if (found.PublishPassword != doc.PublishPassword)
                {
                    ModelState.AddModelError("", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }
                else
                {
                    Session[TryCounterName] = 0;
                    if (userData.GetCache.ContainsKey(found.PublishId))
                    {
                        userData.GetCache[found.PublishId] = found;
                    }
                    else
                    {
                        userData.GetCache.Add(found.PublishId, found);
                    }
                    return RedirectToAction("DownloadPdf", new { publishId = found.PublishId });
                }
                
            }
            Session[TryCounterName] = tryCounter;
            return View("Index");
        }

        public ActionResult DownloadPdf(IUserData userData, string publishId)
        {
            if (userData.GetCache.ContainsKey(publishId))
            {
                var name = string.Format(MainMessages.rs_PDFDocumentDefaultName,
                    userData.GetCache[publishId].CreatDate.ToShortDateString());
                return File(userData.GetCache[publishId].PdfFileData,
                    System.Net.Mime.MediaTypeNames.Application.Pdf, name);
            }
            else
            {
                ModelState.AddModelError("", ValidateMessages.rs_DocumentPleaseEnterPassword);
                return View("Index", new DocumentCredentials
                {
                    PublishId = publishId
                });
            }
            
        }
    }
}