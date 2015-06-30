using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Models.ViewModels;
using PagedList;

namespace NeuronExportedDocuments.Controllers
{
    public class AdminController : Controller
    {
        private const int ServiceMessagesPerPageCount = 3;
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        private IConfig Config { get; set; }
        private IServiceMessages ServiceMessages { get; set; }
        public AdminController(IDBUnitOfWork database, IWebLogger log, IConfig config, IServiceMessages serviceMessages)
        {
            Database = database;
            Log = log;
            Config = config;
            ServiceMessages = serviceMessages;
        }
        public ActionResult Index()
        {
            return View();
        }


        [ActionName("ServiceMessages")]
        public ActionResult ServiceMessagesList(int? page)
        {
            int pageNumber = (page ?? 1);
            var messagesList = Database.ServiceMessages.GetAll().OrderBy(p => p.GetCachedKey());
            var vm = new ServiceMessagesViewModel
            {
                List = messagesList.ToPagedList(pageNumber, ServiceMessagesPerPageCount)
            };


            return View(vm);
        }
    }
}