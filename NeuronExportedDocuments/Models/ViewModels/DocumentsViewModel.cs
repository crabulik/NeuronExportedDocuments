using PagedList;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class DocumentsViewModel
    {
        public DocumentsFilter Filter { get; set; }
        public IPagedList<ServiceDocument> List { get; set; } 
    }
}