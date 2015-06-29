using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Models
{
    public class ServiceDocumentInfo
    {
        public DateTime CreatDate { get; set; }

        public List<DocumentImage> ImagesInterpretation { get; set; }

        public byte[] PdfFileData { get; set; }

        public bool IsPdfExists
        {
            get { return (PdfFileData != null) && (PdfFileData.Length > 0); }
        }

        public byte[] ImageData { get; set; }

        public bool IsImageExists
        {
            get { return (ImageData != null) && (ImageData.Length > 0); }
        }
        public bool IsImagesInZip { get; set; }

        public bool IsBlocked { get; set; }

        public int FailedTimes { get; set; }

        public bool IsOpened { get; set; }

        public DateTime? OpenDate { get; set; }

        public ExportedDocStatus Status { get; set; }

        public string StatusString { get; set; }
        public string PublishId { get; set; }

        public ServiceDocumentInfo()
        {
            IsBlocked = false;
            IsOpened = false;
            Status = ExportedDocStatus.Unhandled;
        } 
    }
}