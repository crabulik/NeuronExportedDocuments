using System.Data.Entity;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.DAL
{
    public class DocumentContext : DbContext
    {
        public DocumentContext()
            : base("DbConnection")
        {
            
        }

        public DbSet<ServiceDocument> ServiceDocuments { get; set; }
        public DbSet<DocumentImage> DocumentImages { get; set; }
    }
}