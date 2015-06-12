using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL.Repositories
{
    public class DocumentImageRepository : IRepository<DocumentImage>
    {
        private DocumentContext _db;

        public DocumentImageRepository(DocumentContext db)
        {
            _db = db;
        }

        public IEnumerable<DocumentImage> GetAll()
        {
            return _db.DocumentImages; 
        }

        public DocumentImage Get(int id)
        {
            return _db.DocumentImages.Find(id);
        }

        public IEnumerable<DocumentImage> Find(Func<DocumentImage, bool> predicate)
        {
            return _db.DocumentImages.Where(predicate).ToList();
        }

        public void Create(DocumentImage item)
        {
            _db.DocumentImages.Add(item); 
        }

        public void Update(DocumentImage item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = _db.DocumentImages.Find(id);
            if (item != null)
                _db.DocumentImages.Remove(item);
        }
    }
}