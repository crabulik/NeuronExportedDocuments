using System.ComponentModel.DataAnnotations;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class AccessDeniedViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string SupportEmail { get; set; }

        public string BackAction { get; set; }
        public string BackController { get; set; }
        
    }
}