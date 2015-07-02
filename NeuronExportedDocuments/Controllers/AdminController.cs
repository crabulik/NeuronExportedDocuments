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
using NeuronExportedDocuments.Models.ViewModels;
using PagedList;

namespace NeuronExportedDocuments.Controllers
{
    public class AdminController : Controller
    {
        private const int LofErrorsPerPageCount = 10;
        private const int ServiceMessagesPerPageCount = 6;
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        private IConfig Config { get; set; }
        private IServiceMessages ServiceMessages { get; set; }
        private IMappingEngine ModelMapper { get; set; }
        public AdminController(IDBUnitOfWork database, IMappingEngine mapper, IWebLogger log, IConfig config, IServiceMessages serviceMessages)
        {
            Database = database;
            Log = log;
            Config = config;
            ServiceMessages = serviceMessages;
            ModelMapper = mapper;
        }
        public ActionResult Index(int? page, NLogErrorsFilter filter = null)
        {
            int pageNumber = (page ?? 1);
            if (filter == null)
                filter = new NLogErrorsFilter();
            var errorsList = Database.NLogErrors.GetAll().Where(w => ((w.RecordTime >= filter.LogStartDateTime) &&
                (w.RecordTime <= filter.LogEndDateTime))).OrderByDescending(p => p.RecordTime);


            var vm = new LogErrorsViewModel
            {
                Filter = filter,
                List = errorsList.ToPagedList(pageNumber, LofErrorsPerPageCount),
            };
            return View(vm);
        }


        [ActionName("ServiceMessages")]
        public ActionResult ServiceMessagesList(int? page)
        {
            int pageNumber = (page ?? 1);
            var messagesList = Database.ServiceMessages.GetAll().OrderBy(p => p.GetCachedKey());
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
        [ValidateAntiForgeryTokenAttribute]
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
    }
}