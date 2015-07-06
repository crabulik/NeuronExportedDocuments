using System;

namespace NeuronExportedDocuments.Models
{
    public class DocumentsFilter
    {
        public DateTime CreateDateStart { get; set; }

        public DateTime CreateDateEnd { get; set; }

        public DocumentsFilter()
        {
            CreateDateStart = DateTime.Now.Date;
            CreateDateEnd = DateTime.Now.Date + new TimeSpan(23, 59, 59);
        }
    }
}