using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class DocumentLogOperationRepository : IRepository<DocumentLogOperation>
    {
        private DocumentContext _db;

        public DocumentLogOperationRepository(DocumentContext db)
        {
            _db = db;
        }

        public IEnumerable<DocumentLogOperation> GetAll()
        {
            return _db.DocumentsLogs;
        }

        public DocumentLogOperation Get(int id)
        {
            return _db.DocumentsLogs.Find(id);
        }

        public IEnumerable<DocumentLogOperation> Find(Func<DocumentLogOperation, bool> predicate)
        {
            return _db.DocumentsLogs.Where(predicate).ToList();
        }

        public void Create(DocumentLogOperation item)
        {
            _db.DocumentsLogs.Add(item); 
        }

        public void Update(DocumentLogOperation item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.DocumentsLogs.Find(id);
            if (item != null)
                _db.DocumentsLogs.Remove(item);
        }
    }
}