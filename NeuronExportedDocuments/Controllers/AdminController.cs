using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NeuronExportedDocuments.Infrastructure.Extensions;
using System.Web.Mvc;
using AutoMapper;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Identity;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Models.ViewModels;
using NeuronExportedDocuments.Resources;
using PagedList;

namespace NeuronExportedDocuments.Controllers
{
    [Authorize(Roles = IdentityConstants.AdminRole)]
    public class AdminController : Controller
    {
        private const int LogErrorsPerPageCount = 10;
        private const int DocumentsPerPageCount = 20;
        private const int ServiceMessagesPerPageCount = 6;
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        private IConfig Config { get; set; }
        private IServiceMessages ServiceMessages { get; set; }
        private IMappingEngine ModelMapper { get; set; }
        private IWebDocumentProcessor DocumentProcessor { get; set; }
        public AdminController(IDBUnitOfWork database, IMappingEngine mapper, IWebLogger log, IConfig config, IServiceMessages serviceMessages,
            IWebDocumentProcessor processor)
        {
            Database = database;
            Log = log;
            Config = config;
            ServiceMessages = serviceMessages;
            ModelMapper = mapper;
            DocumentProcessor = processor;
        }
        public ActionResult Index(int? page, NLogErrorsFilter filter = null)
        {
            int pageNumber = (page ?? 1);
            if (filter == null)
                filter = new NLogErrorsFilter();
            if (filter.LogLevelFilter == LogErrorsViewModel.AllTypesName)
                filter.IsLogLevelFilter = false;


            var errorsList = Database.NLogErrors.GetQueryable().Where(p => ((p.RecordTime >= filter.LogStartDateTime) &&
                                                (p.RecordTime <= filter.LogEndDateTime)));
            if (filter.IsLogLevelFilter)
                errorsList = errorsList.Where(w => w.Level == filter.LogLevelFilter);

            if (filter.IsHostFilter)
                errorsList = errorsList.Where(w => w.Host.Contains(filter.HostFilter));

            errorsList = errorsList.OrderByDescending(p => p.RecordTime);


            var vm = new LogErrorsViewModel
            {
                Filter = filter,
                List = errorsList.ToPagedList(pageNumber, LogErrorsPerPageCount),
                LogLevels = Log.GetAllLogLevels()
            };
            return View(vm);
        }

        public ActionResult NLogErrorXml(int? id)
        {
            if (id.HasValue)
            {
                var result = Database.NLogErrors.Get(id.Value);
                if (result != null)
                    return View((Object)result.AllXml);
            }
            return View((Object)MainMessages.rs_UnknownLogId);
        }

        #region Service Messages

        [ActionName("ServiceMessages")]
        public ActionResult ServiceMessagesList(int? page)
        {
            int pageNumber = (page ?? 1);
            var messagesList = Database.ServiceMessages.GetQueryable().OrderBy(p => p.Key);
            var vm = new ServiceMessagesViewModel
            {
                List = messagesList.ToPagedList(pageNumber, ServiceMessagesPerPageCount),
                ServiceMessages = ServiceMessages
            };


            return View(vm);
        }

        [HttpGet]
        public ActionResult EditServiceMessage(int id, string backUrl)
        {
            var found = Database.ServiceMessages.Get(id);
            if (found == null)
                return PartialView("_ServiceMessageNoFound");
            var viewModel = ModelMapper.Map<EditServiceMessageViewModel>(found);
            viewModel.KeyName = ServiceMessages.GetDefaultKeyDisplayName(viewModel.Key);
            viewModel.DefaultMessage = ServiceMessages.GetDefaultMessage(viewModel.Key);
            if (viewModel.IsDefault)
                viewModel.Message = viewModel.DefaultMessage;
            viewModel.BackUrl = backUrl;
            viewModel.FormaterKeys = ServiceMessages.GetFormatKeys(viewModel.Key);
            return PartialView("_EditServiceMessagePartial", viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditServiceMessage(EditServiceMessageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var toUpdate = Database.ServiceMessages.Get(viewModel.ServiceMessageId);
                toUpdate.IsDefault = viewModel.IsDefault;
                if (!viewModel.IsDefault)
                {
                    toUpdate.Message = viewModel.Message;
                }
                Database.ServiceMessages.Update(toUpdate);
                Database.Save();
                ServiceMessages.UpdateCachedMessage(toUpdate);
            }
            return Redirect(viewModel.BackUrl);
        }

        #endregion

        #region Documents Log
        public ActionResult Documents(int? page, DocumentsFilter filter = null)
        {
            int pageNumber = (page ?? 1);
            if (filter == null)
                filter = new DocumentsFilter();


            var documents = Database.ServiceDocuments.GetQueryable().Where(p => ((p.CreatDate >= filter.CreateDateStart) &&
                                                (p.CreatDate <= filter.CreateDateEnd)));
            if (filter.BlockFilter != BoolFilterStatus.NotSelected)
                documents = documents.Where(p => (filter.BlockFilter == BoolFilterStatus.TrueFilter) ? p.IsBlocked : !p.IsBlocked);

            if (filter.OpenFilter != BoolFilterStatus.NotSelected)
                documents = documents.Where(p => (filter.OpenFilter == BoolFilterStatus.TrueFilter) ? p.IsOpened : !p.IsOpened);

            if (!string.IsNullOrEmpty(filter.PublishIdFilter))
                documents = documents.Where(p => p.PublishId == filter.PublishIdFilter);

            documents = documents.OrderByDescending(p => p.CreatDate);
            


            var vm = new DocumentsViewModel
            {
                Filter = filter,
                List = documents.ToPagedList(pageNumber, DocumentsPerPageCount),
            };
            return View(vm);
        }

        public ActionResult DocumentAdminInfo(int id)
        {
            var found = Database.ServiceDocuments.Get(id);
            if (found == null)
                return PartialView("_DocumentAdminInfoIdError");

            return PartialView("_DocumentAdminInfo", found);

        }

        public ActionResult UnblockAndRepublishDocument(int id, string backUrl)
        {
            var result = new UnblockAndRepublishDocumentResult();
            var doc = Database.ServiceDocuments.Get(id);
            if (doc == null)
            {
                result.Success = false;
                result.ResultMessage = MainExceptionMessages.rs_CantFindTheDocumentForOperation;
                Log.Error(MainExceptionMessages.rs_CantFindTheDocumentForOperation);
            }
            else
            {
                if (doc.Status != ExportedDocStatus.InfoSented)
                {
                    Log.Warn(MainExceptionMessages.rs_CantRepublishDocument, doc);
                    return Redirect(backUrl);
                }
                var changed = false;
                if (doc.IsBlocked)
                {
                    doc.IsBlocked = false;
                    doc.DocumentOperations.Add(
                                new DocumentLogOperation
                                {
                                    OperationType = DocumentLogOperationType.UnblockedByAdmin,
                                    ConnectionIpAddress = Request.UserHostAddress,
                                    Info = string.Format(MainMessages.rs_UserName, User.Identity.Name)
                                });
                }
                if (DocumentProcessor.RepublishDocument(doc))
                {
                    doc.DocumentOperations.Add(
                                new DocumentLogOperation
                                {
                                    OperationType = DocumentLogOperationType.Republished,
                                    ConnectionIpAddress = Request.UserHostAddress,
                                    Info = string.Format(MainMessages.rs_UserName, User.Identity.Name)
                                });
                    Database.ServiceDocuments.Update(doc);
                    Database.Save();
                }
                else
                {
                    result.Success = false;
                    result.ResultMessage = MainExceptionMessages.rs_DocumentWasntRePublished;
                    Log.Warn(MainExceptionMessages.rs_DocumentWasntRePublished, doc);
                }
                    
            }

            return Redirect(backUrl);
        }
        #endregion


        public ActionResult ConfirmDialog(ConfirmDialogViewModel viewModel)
        {
            
            return PartialView("_ConfirmDialog", viewModel);
        }
    }
}