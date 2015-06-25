namespace NeuronExportedDocuments.Models.ViewModels
{
    public class GetDocumentViewModel
    {
        public ServiceDocumentInfo ServiceDocumentInfo { get; private set; }

        public int AccessDaysCount { get; private set; }

        public GetDocumentViewModel(ServiceDocumentInfo serviceDocumentInfo, int accessDaysCount)
        {
            ServiceDocumentInfo = serviceDocumentInfo;
            AccessDaysCount = accessDaysCount;
        }
    }
}