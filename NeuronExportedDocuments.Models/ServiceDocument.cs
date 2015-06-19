using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Models
{
    public class ServiceDocument
    {
        public int ServiceDocumentId { get; set; }

        public int NeuronDbDocumentId { get; set; }

        public string Name { get; set; }

        public string DeliveryEMail { get; set; }

        public string DeliveryPhone { get; set; }

        public DateTime CreatDate { get; set; }

        public List<DocumentImage> ImagesInterpretation { get; set; }

        public byte[] PdfFileData { get; set; }

        public bool IsBlocked { get; set; }

        public int FailedTimes { get; set; }

        public bool IsOpened { get; set; }

        public DateTime? OpenDate { get; set; }

        public ExportedDocStatus Status { get; set; }
        [MaxLength(12)]
        public string PublishId { get; set; }
        [MaxLength(8)]
        public string PublishPassword { get; set; }

        public ServiceDocument()
        {
            IsBlocked = false;
            IsOpened = false;
            Status = ExportedDocStatus.Unhandled;
        }
    }
}