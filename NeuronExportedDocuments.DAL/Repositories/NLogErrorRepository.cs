using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class NLogErrorRepository : IRepository<NLogError>
    {
        private DocumentContext _db;

        public NLogErrorRepository(DocumentContext db)
        {
            _db = db;
            _db.Database.Log = p => System.Diagnostics.Debug.Write(p);
        }

        public IEnumerable<NLogError> GetAll()
        {
            return _db.NLogErrors;
        }

        public IQueryable<NLogError> GetQueryable()
        {
            return _db.NLogErrors;
        }

        public NLogError Get(int id)
        {
            return _db.NLogErrors.Find(id);
        }

        public IEnumerable<NLogError> Find(Func<NLogError, bool> predicate)
        {
            return _db.NLogErrors.Where(predicate).ToList();
        }

        public void Create(NLogError item)
        {
            _db.NLogErrors.Add(item); 
        }

        public void Update(NLogError item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.NLogErrors.Find(id);
            if (item != null)
                _db.NLogErrors.Remove(item);
        }
    }
}