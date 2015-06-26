using System;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Interfaces
{
    public interface IDBUnitOfWork : IDisposable
    {
        IRepository<ServiceDocument> ServiceDocuments { get; }
        IRepository<DocumentImage> DocumentImages { get; }
        IRepository<NLogError> NLogErrors { get; }
        IRepository<ServiceMessage> ServiceMessages { get; }

        IRepository<DocumentLogOperation> DocumentsLogs { get; }
        void Save();
    }
}