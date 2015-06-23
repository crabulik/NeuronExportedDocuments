using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
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
        protected const string PngMimeName = "image/png";
        private IUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        private IMappingEngine ModelMapper { get; set; }

        
        public HomeController(IUnitOfWork uow, IWebLogger logger, IMappingEngine mapper)
        {
            Database = uow;
            Log = logger;
            ModelMapper = mapper;
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
                    var documentInfo = ModelMapper.Map<ServiceDocumentInfo>(found);
                    if (userData.GetCache.ContainsKey(documentInfo.PublishId))
                    {

                        userData.GetCache[documentInfo.PublishId] = documentInfo;
                    }
                    else
                    {
                        userData.GetCache.Add(documentInfo.PublishId, documentInfo);
                    }
                    return View(documentInfo);
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
                    userData.GetCache[publishId].PublishId);
                return File(userData.GetCache[publishId].PdfFileData,
                    System.Net.Mime.MediaTypeNames.Application.Pdf, name);
            }
            else
            {
                return ToGetPageWithFilledPublishId(publishId);
            }
            
        }

        public ActionResult DownloadImg(IUserData userData, string publishId)
        {
            if (userData.GetCache.ContainsKey(publishId))
            {
                var doc = userData.GetCache[publishId];
                if (doc.IsImagesInZip)
                {
                    var name = string.Format(MainMessages.rs_ZIPDocumentDefaultName,
                        doc.PublishId);
                    return File(doc.ImageData,
                        System.Net.Mime.MediaTypeNames.Application.Zip, name);
                }
                else
                {
                    var name = string.Format(MainMessages.rs_PNGDocumentDefaultName,
                        doc.PublishId);
                    return File(doc.ImageData,
                        PngMimeName, name);
                }
            }
            else
            {
                return ToGetPageWithFilledPublishId(publishId);
            }
        }

        private ActionResult ToGetPageWithFilledPublishId(string publishId)
        {
            ModelState.AddModelError("", ValidateMessages.rs_DocumentPleaseEnterPassword);
            return View("Index", new DocumentCredentials
            {
                PublishId = publishId
            });
        }
    }
}