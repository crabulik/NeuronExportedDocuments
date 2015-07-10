namespace NeuronExportedDocuments.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public DocumentCredentials DocumentCredentials { get; set; }

        public string HelloMessage { get; set; }
        public string HelloDescriptionMessage { get; set; }

        public bool IsNeedCaptcha { get; set; }

        public string RecaptchaSiteKey { get; set; }
        
    }
}