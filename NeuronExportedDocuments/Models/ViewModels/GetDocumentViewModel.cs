namespace NeuronExportedDocuments.Models.ViewModels
{
    public class GetDocumentViewModel
    {
        public ServiceDocumentInfo ServiceDocumentInfo { get; private set; }

        public int AccessDaysCount { get; private set; }

        public string AccessFinalDate
        {
            get
            {
                if ((ServiceDocumentInfo != null) && (ServiceDocumentInfo.IsOpened) && (ServiceDocumentInfo.OpenDate.HasValue))
                {
                    return ServiceDocumentInfo.OpenDate.Value.AddDays(AccessDaysCount).ToShortDateString();
                }

                return string.Empty;
            }
        }

        public string WarningMessage { get; set; }

        public GetDocumentViewModel(ServiceDocumentInfo serviceDocumentInfo, int accessDaysCount)
        {
            ServiceDocumentInfo = serviceDocumentInfo;
            AccessDaysCount = accessDaysCount;
        }
    }
}