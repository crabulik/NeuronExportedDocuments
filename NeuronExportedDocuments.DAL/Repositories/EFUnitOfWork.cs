using System;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class EFUnitOfWork : IDisposable, IUnitOfWork
    {
        private DocumentContext _db;
        private ServiceDocumentRepository _serviceDocumentRepository;
        private DocumentImageRepository _documentImageRepository;

        public EFUnitOfWork()
        {
            _db = new DocumentContext();
        }
        public IRepository<ServiceDocument> ServiceDocuments
        {
            get
            {
                if (_serviceDocumentRepository == null)
                    _serviceDocumentRepository = new ServiceDocumentRepository(_db);
                return _serviceDocumentRepository;
            }
        }

        public IRepository<DocumentImage> DocumentImages
        {
            get
            {
                if (_documentImageRepository == null)
                    _documentImageRepository = new DocumentImageRepository(_db);
                return _documentImageRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}