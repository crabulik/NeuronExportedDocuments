using PagedList;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class ServiceMessagesViewModel
    {
        public IPagedList<ServiceMessage> List { get; set; }
         
    }
}