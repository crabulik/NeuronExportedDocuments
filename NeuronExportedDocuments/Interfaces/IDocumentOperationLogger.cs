using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IDocumentOperationLogger
    {
        void LogDocument(ServiceDocument document, DocumentLogOperationType operation,
            string connectionIpAddress, string info); 
    }
}