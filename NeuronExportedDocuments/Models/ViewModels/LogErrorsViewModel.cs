
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NeuronExportedDocuments.Resources;
using PagedList;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class LogErrorsViewModel
    {
        public static string AllTypesName
        {
            get { return MainMessages.rs_All; }
        }

        public List<string> LogLevels { get; set; }

        public List<SelectListItem> GetLogLevelsFilterList()
        {

            var result = new List<SelectListItem>((LogLevels == null) ? 1 : LogLevels.Count + 1);
            result.Add(new SelectListItem { Text = AllTypesName, Value = AllTypesName});
            if (LogLevels != null)
                result.AddRange(LogLevels.Select(item => new SelectListItem { Text = item, Value = item }));
            return result;
        }
        public NLogErrorsFilter Filter { get; set; }
        public IPagedList<NLogError> List { get; set; }
    }
}