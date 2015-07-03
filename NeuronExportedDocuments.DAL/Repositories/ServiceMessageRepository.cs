using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class ServiceMessageRepository : IRepository<ServiceMessage>
    {
        private DocumentContext _db;

        public ServiceMessageRepository(DocumentContext db)
        {
            _db = db;
        }
        public IEnumerable<ServiceMessage> GetAll()
        {
            return _db.ServiceMessages;
        }

        public IQueryable<ServiceMessage> GetQueryable()
        {
            return _db.ServiceMessages;
        }

        public ServiceMessage Get(int id)
        {
            return _db.ServiceMessages.Find(id);
        }

        public IEnumerable<ServiceMessage> Find(Func<ServiceMessage, bool> predicate)
        {
            return _db.ServiceMessages.Where(predicate).ToList();
        }

        public void Create(ServiceMessage item)
        {
            throw new MethodAccessException(
                "Create method is not allowed for ServiceMessage object");
        }

        public void Update(ServiceMessage item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            throw new MethodAccessException(
                "Delete method is not allowed for ServiceMessage object");
        }
    }
}