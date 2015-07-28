using System;
using System.Data.Entity.Validation;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Identity;
using NeuronExportedDocuments.Models.Interfaces;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class EFUnitOfWork : IDisposable, IDBUnitOfWork
    {
        private DocumentContext _db;
        private ServiceDocumentRepository _serviceDocumentRepository;
        private DocumentImageRepository _documentImageRepository;
        private NLogErrorRepository _nLogErroreRepository;
        private DocumentLogOperationRepository _documentLogOperationRepository;
        private ServiceMessageRepository _serviceMessageRepository;

        private IWebLogger Log { get; set; }

        public EFUnitOfWork(IWebLogger logger)
        {
            _db = new DocumentContext();
            Log = logger;
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

        public IRepository<NLogError> NLogErrors
        {
            get
            {
                if (_nLogErroreRepository == null)
                    _nLogErroreRepository = new NLogErrorRepository(_db);
                return _nLogErroreRepository;
            }
        }

        public IRepository<DocumentLogOperation> DocumentsLogs
        {
            get
            {
                if (_documentLogOperationRepository == null)
                    _documentLogOperationRepository = new DocumentLogOperationRepository(_db);
                return _documentLogOperationRepository;
            }
        }

        public IRepository<ServiceMessage> ServiceMessages
        {
            get
            {
                if (_serviceMessageRepository == null)
                    _serviceMessageRepository = new ServiceMessageRepository(_db);
                return _serviceMessageRepository;
            }
        }

        public void Save()
        {
            
            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    var sb = new StringBuilder("Entity of type \"");
                    sb.Append(eve.Entry.Entity.GetType().Name);
                    sb.Append("\" in state \"");
                    sb.Append(eve.Entry.State.ToString());
                    sb.AppendLine("\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.Append("- Property: \"");
                        sb.Append(ve.PropertyName);
                        sb.Append("\", Error: \"");
                        sb.Append(ve.ErrorMessage);
                        sb.AppendLine("\"");
                    }
                    Log.Error(sb.ToString(), e);
                }
                throw;
            }
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

        public IdentityDbContext<ApplicationUser> GetIdentityDbContext()
        {
            return _db;
        }
    }
}