namespace NeuronExportedDocuments.Models.ViewModels
{
    public class BlockedViewModel
    {
        public ServiceDocumentInfo ServiceDocumentInfo { get; private set; }

        public string SupportContact { get; private set; }

        public BlockedViewModel(ServiceDocumentInfo serviceDocumentInfo, string supportContact)
        {
            ServiceDocumentInfo = serviceDocumentInfo;
            SupportContact = supportContact;
        }
    }
}