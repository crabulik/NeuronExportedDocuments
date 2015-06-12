using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class ServiceDocumentRepository : IRepository<ServiceDocument>
    {
        private DocumentContext _db;

        public ServiceDocumentRepository(DocumentContext db)
        {
            _db = db;
        }
        public IEnumerable<ServiceDocument> GetAll()
        {
            return _db.ServiceDocuments;
        }

        public ServiceDocument Get(int id)
        {
            return _db.ServiceDocuments.Find(id);
        }

        public IEnumerable<ServiceDocument> Find(Func<ServiceDocument, bool> predicate)
        {
            return _db.ServiceDocuments.Where(predicate).ToList();
        }

        public void Create(ServiceDocument item)
        {
            _db.ServiceDocuments.Add(item); 
        }

        public void Update(ServiceDocument item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.ServiceDocuments.Find(id);
            if (item != null)
                _db.ServiceDocuments.Remove(item);
        }
    }
}