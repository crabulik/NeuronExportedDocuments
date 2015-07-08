using System;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Models
{
    public class DocumentsFilter
    {
        public DateTime CreateDateStart { get; set; }

        public DateTime CreateDateEnd { get; set; }

        public BoolFilterStatus BlockFilter { get; set; }

        public BoolFilterStatus OpenFilter { get; set; }

        public string PublishIdFilter { get; set; }

        public DocumentsFilter()
        {
            CreateDateStart = DateTime.Now.Date;
            CreateDateEnd = DateTime.Now.Date + new TimeSpan(23, 59, 59);
            BlockFilter = BoolFilterStatus.NotSelected;
            OpenFilter = BoolFilterStatus.NotSelected;
            PublishIdFilter = string.Empty;
        }
    }
}