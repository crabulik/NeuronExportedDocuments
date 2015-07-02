using PagedList;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class LogErrorsViewModel
    {
        public NLogErrorsFilter Filter { get; set; }
        public IPagedList<NLogError> List { get; set; }
    }
}