using System;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ServiceDocument> ServiceDocuments { get; }
        IRepository<DocumentImage> DocumentImages { get; }
        IRepository<NLogError> NLogErrors { get; }
        void Save();
    }
}