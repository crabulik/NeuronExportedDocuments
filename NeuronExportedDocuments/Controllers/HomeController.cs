using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
using Newtonsoft.Json;

namespace NeuronExportedDocuments.Controllers
{
    public class HomeController : Controller
    {
        enum CheckDocumentResult
        {
            Ok,
            Blocked,
            CaptchaError,
            Error
        }

        protected const string PngMimeName = "image/png";
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        private IDocumentOperationLogger DocumentLog { get; set; }
        private IMappingEngine ModelMapper { get; set; }
        private IConfig Config { get; set; }
        private IServiceMessages ServiceMessages { get; set; }

        private IWebDocumentProcessor DocumentProcessor { get; set; }
        
        public HomeController(IDBUnitOfWork uow, IWebLogger logger, IDocumentOperationLogger documentLogger, IMappingEngine mapper,
            IConfig config, IServiceMessages serviceMessages, IWebDocumentProcessor documentProcessor)
        {
            Database = uow;
            Log = logger;
            DocumentLog = documentLogger;
            ModelMapper = mapper;
            Config = config;
            ServiceMessages = serviceMessages;
            DocumentProcessor = documentProcessor;
        }
        public ActionResult Index(IUserData userData)
        {
            var tmpVM = new HomeIndexViewModel
            {
                DocumentCredentials = new DocumentCredentials
                {
                    IsWithCaptcha = (userData.FailTryCount >= Config.GeneralSettings.FailSiteAccessForCaptcha)
                },
                HelloMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloMessage),
                HelloDescriptionMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloDescriptionMessage),
                RecaptchaSiteKey = Config.GeneralSettings.RecaptchaSiteKey
            };
            return View(tmpVM);
        }

        public async Task<ActionResult> GetDocument(IUserData userData, DocumentCredentials doc)
        {
            bool? captchaCheck = null;
            var tmpIsNeedCaptcha = false;
            if (ModelState.IsValid)
            {
                if (doc.IsWithCaptcha || (userData.FailTryCount >= Config.GeneralSettings.FailSiteAccessForCaptcha))
                {                
                    captchaCheck = await ValidateCaptcha();
                }

                if ((captchaCheck == null) || captchaCheck.Value)
                {
                    var found =
                        Database.ServiceDocuments.GetQueryable()
                            .FirstOrDefault(document => document.PublishId == doc.PublishId);
                    var checkResult = CheckDocument(found, userData, doc.PublishId, doc.PublishPassword, captchaCheck);
                    switch (checkResult)
                    {
                        case CheckDocumentResult.Ok:
                        {
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

                            var tmpVm = new GetDocumentViewModel(documentInfo, Config.GeneralSettings.DocumentAccessDaysCount);
                            tmpVm.WarningMessage =
                                ServiceMessages.FormatMessageByGetDocumentViewModel(
                                    ServiceMessageKey.GetDocumentWarningMessage, tmpVm);
                            return View(tmpVm);
                        }
                        case CheckDocumentResult.Blocked:
                            var blockVm = new BlockedViewModel(ModelMapper.Map<ServiceDocumentInfo>(found),
                                Config.GeneralSettings.SupportEmail);
                            return View("BlockedInfo", blockVm);
                        case CheckDocumentResult.CaptchaError:
                            tmpIsNeedCaptcha = true;
                            break;
                        case CheckDocumentResult.Error:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                }
                else
                {
                }

            }
            else
            {
                ModelState.AddModelError("", ValidateMessages.rs_DocumentIsNotValid);
            }
            doc.IsWithCaptcha = tmpIsNeedCaptcha ||
                                (userData.FailTryCount >= Config.GeneralSettings.FailSiteAccessForCaptcha);
            var tmpIndexVm = new HomeIndexViewModel
            {
                DocumentCredentials = doc,
                HelloMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloMessage),
                HelloDescriptionMessage = ServiceMessages.GetMessage(ServiceMessageKey.HomeIndexHelloDescriptionMessage),
                RecaptchaSiteKey = Config.GeneralSettings.RecaptchaSiteKey
            };
            return View("Index", tmpIndexVm);
        }

        private CheckDocumentResult CheckDocument(ServiceDocument doc, IUserData userData, string docId, string password, bool? captchaCheck)
        {
            var result = CheckDocumentResult.Error;
            if (doc == null)
            {
                Log.Info(string.Format(MainMessages.rs_RequestForUnexistedDocument, docId,
                    Request.UserHostAddress));
                userData.FailTryCount += 1;
                ModelState.AddModelError("", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
            }
            else
            {
                if ((doc.FailedTimes >= Config.GeneralSettings.FailDocumentAccessForCaptcha) &&
                    ((captchaCheck == null) || !captchaCheck.Value))
                {
                    ModelState.AddModelError("", ValidateMessages.rs_CaptchaErrorValue);
                    return CheckDocumentResult.CaptchaError;
                }

                if (doc.PublishPassword == password)
                {
                    if(doc.IsBlocked)
                        return CheckDocumentResult.Blocked;
                    if (doc.Status == ExportedDocStatus.InArchive)
                    {
                        ModelState.AddModelError("", ValidateMessages.rs_DocumentIsInArchive);
                        return CheckDocumentResult.Error;
                    }
                    return CheckDocumentResult.Ok;
                }
                else
                {
                    SetDocumentFailAccess(doc);
                    userData.FailTryCount += 1;
                    if (doc.FailedTimes >= Config.GeneralSettings.FailAccessCountForBan)
                    {
                        if (!doc.IsBlocked)
                            DocumentProcessor.BlockDocument(doc, Request.UserHostAddress);
                        return CheckDocumentResult.Blocked;
                    }
                    ModelState.AddModelError("", ValidateMessages.rs_DocumentIdOrPasswordAreIncorrect);
                }

            }
            return result;
        }

        private async Task<bool> ValidateCaptcha()
        {
            var response = Request["g-recaptcha-response"];

            var secret = Config.GeneralSettings.RecaptchaSecretKey;

            var requestUrl =
            String.Format(
                "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}",
                secret,
                response,
                Request.UserHostAddress);
            string result;
            using (var client = new HttpClient())
            {
                result = await client.GetStringAsync(requestUrl);
            }

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(result);

            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0)
                {
                    ModelState.AddModelError("", ValidateMessages.rs_CaptchaErrorValue);
                    return false;
                }

                ModelState.AddModelError("", ValidateMessages.rs_CaptchaErrorOccured);
                var error = captchaResponse.ErrorCodes[0].ToLower();
                string logError = string.Empty;
                switch (error)
                {
                    case ("missing-input-secret"):
                        logError = ValidateMessages.rs_CaptchaErrorSecretParameterIsMissing;
                        break;
                    case ("invalid-input-secret"):
                        logError = ValidateMessages.rs_CaptchaErrorSecretParameterIsInvalid;
                        break;

                    case ("missing-input-response"):
                        logError = ValidateMessages.rs_CaptchaErrorResponseParameterMissing;
                        break;
                    case ("invalid-input-response"):
                        logError = ValidateMessages.rs_CaptchaErrorResponseParameterInvalid;
                        break;

                    default:
                        logError = ValidateMessages.rs_CaptchaErrorUnknown;
                        break;
                }
                Log.Fatal(string.Format(ValidateMessages.rs_CaptchaError, logError));
            }
            else
            {
                return true;
            }

            return false;
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