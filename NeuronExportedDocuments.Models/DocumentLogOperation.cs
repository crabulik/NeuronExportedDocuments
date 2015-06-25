using System;
using System.ComponentModel.DataAnnotations;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Models
{
    public class DocumentLogOperation
    {
        private const string NoAdditionalInfoString = "<NoInfo>";
        public int DocumentLogOperationId { get; set; }
        public int ServiceDocumentId { get; set; }

        public DocumentLogOperationType OperationType { get; set; }

        public DateTime OperationDate { get; set; }
        [Required]
        public string ConnectionIpAddress { get; set; }

        [MaxLength]
        [Required]
        public string Info { get; set; }


        public DocumentLogOperation()
        {
            OperationDate = DateTime.Now;
            ConnectionIpAddress = string.Empty;
            Info = NoAdditionalInfoString;
        }
    }
}