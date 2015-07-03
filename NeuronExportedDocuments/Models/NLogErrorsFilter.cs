using System;

namespace NeuronExportedDocuments.Models
{
    public class NLogErrorsFilter
    {
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }

        public bool IsHostFilter { get; set; }
        public string HostFilter { get; set; }

        public bool IsLogLevelFilter { get; set; }
        public string LogLevelFilter { get; set; }

        public NLogErrorsFilter()
        {
            IsHostFilter = false;
            IsLogLevelFilter = false;
            LogStartDateTime = DateTime.Now.Date;
            LogEndDateTime = DateTime.Now.Date + new TimeSpan(23, 59, 59);
        }
    }
}