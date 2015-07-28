using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Identity;

namespace NeuronExportedDocuments.DAL
{
    public class DocumentContext : IdentityDbContext<ApplicationUser>
    {
        public DocumentContext()
            : base("name=DocumentsDb", throwIfV1Schema: false)
        {
            
        }

        public DbSet<ServiceDocument> ServiceDocuments { get; set; }
        public DbSet<DocumentImage> DocumentImages { get; set; }
        public DbSet<NLogError> NLogErrors { get; set; }
        public DbSet<DocumentLogOperation> DocumentsLogs { get; set; }
        public DbSet<ServiceMessage> ServiceMessages { get; set; }
    }
}