using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class EditServiceMessageViewModel
    {
        public int ServiceMessageId { get; set; }

        public ServiceMessageKey Key { get; set; }

        public string KeyName { get; set; }

        [Display(Name = "rs_EditServiceMessageName", ResourceType = typeof(MainMessages))]
        public string Message { get; set; }

        [Display(Name = "rs_EditServiceIsDefaultName", ResourceType = typeof(MainMessages))]
        public bool IsDefault { get; set; }

        public string DefaultMessage { get; set; }
        [Display(Name = "rs_FormaterKeysName", ResourceType = typeof(MainMessages))]
        public List<FormaterKey> FormaterKeys { get; set; }

        public string BackUrl { get; set; }
    }
}