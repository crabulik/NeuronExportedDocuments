namespace NeuronExportedDocuments.Models
{
    public class DocumentImage
    {
        public int DocumentImageId { get; set; }
        public int ServiceDocumentId { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }

        public byte[] ImageData { get; set; } 
    }
}