using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Controllers
{
    public class DocumentPublisherController : ApiController
    {
        private IUnitOfWork Database { get; set; }
        private IWebDocumentProcessor DocumentProcessor { get; set; }
        static object _synclock = new object();

        public DocumentPublisherController(IUnitOfWork database, IWebDocumentProcessor processor)
        {
            Database = database;
            DocumentProcessor = processor;
        }





        public bool Get()
        {
            var result = false;
            lock (_synclock)
            {
                var changed = false;
                var unhandledDocs =
                    Database.ServiceDocuments.Find((document) => document.Status == ExportedDocStatus.Unhandled);
                foreach (var unhandledDoc in unhandledDocs)
                {
                    if (DocumentProcessor.PublishDocument(unhandledDoc))
                    {
                        Database.ServiceDocuments.Update(unhandledDoc);
                        changed = true;
                    }
                }
                if (changed)
                    Database.Save();

                changed = false;
                var publishedDocs =
                    Database.ServiceDocuments.Find((document) => document.Status == ExportedDocStatus.Published);
                foreach (var publishedDoc in publishedDocs)
                {
                    if (DocumentProcessor.SendDocInfo(publishedDoc))
                    {
                        Database.ServiceDocuments.Update(publishedDoc);
                        changed = true;
                    }
                }
                if (changed)
                {
                    Database.Save();
                    result = true;
                }
            }

            return result;
        }
    }
}