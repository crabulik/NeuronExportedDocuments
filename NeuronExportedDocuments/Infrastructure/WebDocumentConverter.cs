using System.Collections.Generic;
using NeuronDocumentSync.Models;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.Infrastructure
{
    public class WebDocumentConverter
    {
        public ServiceDocument Convert(ExportServiceDocument document)
        {
            if (document == null)
                return null;
            var result = new ServiceDocument
            {
                CreatDate = document.CreatDate,
                DeliveryEMail = document.DeliveryEMail,
                DeliveryPhone = document.DeliveryPhone,
                NeuronDbDocumentId = document.NeuronDbDocumentId,
                Name = document.Name,
                PdfFileData = document.PdfFileData,
                ImagesInterpretation = new List<DocumentImage>()
            };
            foreach (var img in document.ImagesInterpretation)
            {
                result.ImagesInterpretation.Add(new DocumentImage
                {
                    FileName = img.FileName,
                    MimeType = img.MimeType,
                    ImageData = img.ImageData
                });
            }

            return result;
        }
    }
}