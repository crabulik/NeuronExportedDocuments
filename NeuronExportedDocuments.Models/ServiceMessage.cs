using System.ComponentModel.DataAnnotations;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Models
{
    public class ServiceMessage
    {
        public int ServiceMessageId { get; set; }

        [Required]
        public ServiceMessageKey Key { get; set; }

        [MaxLength]
        public string Message { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public ServiceMessage()
        {
            IsDefault = true;
        }

        public string GetCachedKey()
        {
            return "ServiceMessage_" + Key.ToString();
        }
    }
}