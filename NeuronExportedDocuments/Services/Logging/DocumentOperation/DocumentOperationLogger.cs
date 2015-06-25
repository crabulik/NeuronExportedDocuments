using System;
using System.Collections.Generic;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Services.Logging.DocumentOperation
{

    public class DocumentOperationLogger : IDocumentOperationLogger
    {
        private IDBUnitOfWork Database { get; set; }
        private IWebLogger Log { get; set; }
        public DocumentOperationLogger(IDBUnitOfWork uow, IWebLogger logger)
        {
            Database = uow;
            Log = logger;
        }

        public void LogDocument(ServiceDocument document, DocumentLogOperationType operation,
            string connectionIpAddress, string info)
        {
            if (document.DocumentOperations == null)
            {
                document.DocumentOperations = new List<DocumentLogOperation>();
            }
            var logOperation = new DocumentLogOperation
                {
                    DocumentLogOperationId = document.ServiceDocumentId,
                    OperationDate = DateTime.Now,
                    OperationType = operation,
                    ConnectionIpAddress = connectionIpAddress,
                    Info = info
                };
            try
            {            
                
                document.DocumentOperations.Add(logOperation);
                Database.DocumentsLogs.Create(logOperation);
                Database.Save();

            }
            catch (Exception ex)
            {
                Log.Error(string.Format(MainExceptionMessages.rs_ExceptionForOperationLog, LogUtility.BuildDocumentMessage(document), 
                    LogUtility.BuildDocumentLogOperationMessage(logOperation)), ex);
                throw;
            }
        }
    }
}