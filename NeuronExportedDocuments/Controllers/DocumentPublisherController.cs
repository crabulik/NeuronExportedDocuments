using System;
using System.Linq;
using System.Web.Http;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Infrastructure.Extensions;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Controllers
{
    public class DocumentPublisherController : ApiController
    {
        private IDBUnitOfWork Database { get; set; }
        private IWebDocumentProcessor DocumentProcessor { get; set; }
        private IWebLogger Log { get; set; }
        static object _synclock = new object();

        public DocumentPublisherController(IDBUnitOfWork database, IWebDocumentProcessor processor, IWebLogger logger)
        {
            Database = database;
            DocumentProcessor = processor;
            Log = logger;
        }


        public bool Get()
        {
            var result = false;
            lock (_synclock)
            {
                try
                {
                    var changed = false;
                    var unhandledDocs =
                        Database.ServiceDocuments.GetQueryable()
                            .Where((document) => document.Status == ExportedDocStatus.Unhandled);
                    foreach (var unhandledDoc in unhandledDocs)
                    {
                        if (DocumentProcessor.PublishDocument(unhandledDoc))
                        {
                            unhandledDoc.DocumentOperations.Add(
                                new DocumentLogOperation
                                {
                                    OperationType = DocumentLogOperationType.Published,
                                    ConnectionIpAddress = Request.GetClientIpAddress()
                                });
                            Database.ServiceDocuments.Update(unhandledDoc);
                            changed = true;
                        }
                        else
                        {
                            Log.Warn(MainExceptionMessages.rs_DocumentWasntPublished, unhandledDoc);
                        }
                    }
                    if (changed)
                        Database.Save();

                    changed = false;
                    var publishedDocs =
                        Database.ServiceDocuments.GetQueryable().Where((document) => document.Status == ExportedDocStatus.Published);
                    foreach (var publishedDoc in publishedDocs)
                    {
                        if (DocumentProcessor.SendDocInfo(publishedDoc))
                        {
                            publishedDoc.DocumentOperations.Add(
                                new DocumentLogOperation
                                {
                                    OperationType = DocumentLogOperationType.InfoSentedToUser,
                                    ConnectionIpAddress = Request.GetClientIpAddress()
                                });
                            Database.ServiceDocuments.Update(publishedDoc);
                            changed = true;
                        }
                        else
                        {
                            Log.Warn(MainExceptionMessages.rs_DocumentInfoWasntSendedToUser, publishedDoc);
                        }
                    }
                    if (changed)
                    {
                        Database.Save();
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(MainExceptionMessages.rs_ExceptionInDocumentPublisherControllerGet, e);
                    return false;
                }
            }

            return result;
        }
    }
}