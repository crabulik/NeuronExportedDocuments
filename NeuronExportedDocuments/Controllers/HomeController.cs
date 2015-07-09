using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Models.ViewModels;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Controllers
{
    public class HomeController : Controller
    {
        protected const string TryCounterName = "TryCounterName";
        protected const string PngMimeName = "image/png";
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        private IDocumentOperationLogger DocumentLog { get; set; }
        private IMappingEngine ModelMapper { get; set; }
        private IConfig Config { get; set; }
        private IServiceMessages ServiceMessages { get; set; }

        
        public HomeController(IDBUnitOfWork uow, IWebLogger logger, IDocumentOperationLogger documentLogger, IMappingEngine mapper,
            IConfig config, IServiceMessages serviceMessages)
        {
            Database = uow;
            Log = logger;
            DocumentLog = documentLogger;
            ModelMapper = mapper;
            Config = config;
            ServiceMessages = serviceMessages;
        }
        public ActionResult Index()
        {
            var tmpVM = new HomeIndexViewModel
            {
                DocumentCredentials = new DocumentCredentials(),
                HelloMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloMessage),
                HelloDescriptionMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloDescriptionMessage)
            };
            return View(tmpVM);
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
                var found = Database.ServiceDocuments.GetQueryable().FirstOrDefault(document => document.PublishId == doc.PublishId);
                if (found == null)
                {
                    Log.Info(string.Format(MainMessages.rs_RequestForUnexistedDocument, doc.PublishId, Request.UserHostAddress));

                    ModelState.AddModelError("", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }
                else if (found.PublishPassword != doc.PublishPassword)
                {
                    SetDocumentFailAccess(found);                   

                    ModelState.AddModelError("", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }
                else if (found.Status == ExportedDocStatus.InArchive)
                {
                    ModelState.AddModelError("", ValidateMessages.rs_DocumentIsInArchive);
                }
                else
                {
                    Session[TryCounterName] = 0;
                    SetDocumentOpened(found);
                    var documentInfo = ModelMapper.Map<ServiceDocumentInfo>(found);
                    if (userData.GetCache.ContainsKey(documentInfo.PublishId))
                    {

                        userData.GetCache[documentInfo.PublishId] = documentInfo;
                    }
                    else
                    {
                        userData.GetCache.Add(documentInfo.PublishId, documentInfo);
                    }

                    var tmpVM = new GetDocumentViewModel(documentInfo, Config.GeneralSettings.DocumentAccessDaysCount);
                    tmpVM.WarningMessage = ServiceMessages.FormatMessageByGetDocumentViewModel(ServiceMessageKey.GetDocumentWarningMessage, tmpVM);
                    return View(tmpVM);
                }
                
            }

            var tmpIndexVM = new HomeIndexViewModel
            {
                DocumentCredentials = doc,
                HelloMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloMessage),
                HelloDescriptionMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloDescriptionMessage)
            };
            Session[TryCounterName] = tryCounter;
            return View("Index", tmpIndexVM);
        }

        private void SetDocumentFailAccess(ServiceDocument document)
        {
            document.FailedTimes ++;

            document.DocumentOperations.Add(new DocumentLogOperation
            {
                OperationType = DocumentLogOperationType.FailAccess,
                ConnectionIpAddress = Request.UserHostAddress
            });
            Database.ServiceDocuments.Update(document);
            Database.Save();

            Log.Info(string.Format(MainMessages.rs_RequestForDocumentWithWrongPassword, document.PublishId, Request.UserHostAddress));
        }

        private void SetDocumentOpened(ServiceDocument document)
        {
            document.FailedTimes = 0;
            if (!document.IsOpened)
            {
                document.IsOpened = true;
                document.OpenDate = DateTime.Now;               
            }
            document.DocumentOperations.Add( new DocumentLogOperation
            {
                OperationType = DocumentLogOperationType.SuccessAccess,
                ConnectionIpAddress = Request.UserHostAddress
            });
            Database.ServiceDocuments.Update(document);
            Database.Save();
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