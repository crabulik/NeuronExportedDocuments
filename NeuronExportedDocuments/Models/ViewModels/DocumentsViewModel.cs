using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Web.Mvc;

using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Resources;
using PagedList;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class DocumentsViewModel
    {
        public List<SelectListItem> GetBlockFilterList()
        {

            var result = new List<SelectListItem>(3);
            result.Add(new SelectListItem { Text = MainMessages.rs_DocumentBlockFilterOff, Value = ((int)BoolFilterStatus.NotSelected).ToString() });
            result.Add(new SelectListItem { Text = MainMessages.rs_DocumentTrueBlockFilter, Value = ((int)BoolFilterStatus.TrueFilter).ToString() });
            result.Add(new SelectListItem { Text = MainMessages.rs_DocumentFalseBlockFilter, Value = ((int)BoolFilterStatus.FalseFilter).ToString() });

            return result;
        }

        public List<SelectListItem> GetOpenFilterList()
        {

            var result = new List<SelectListItem>(3);
            result.Add(new SelectListItem { Text = MainMessages.rs_DocumentOpenFilterOff, Value = ((int)BoolFilterStatus.NotSelected).ToString() });
            result.Add(new SelectListItem { Text = MainMessages.rs_DocumentTrueOpenFilter, Value = ((int)BoolFilterStatus.TrueFilter).ToString() });
            result.Add(new SelectListItem { Text = MainMessages.rs_DocumentFalseOpenFilter, Value = ((int)BoolFilterStatus.FalseFilter).ToString() });

            return result;
        }
        public DocumentsFilter Filter { get; set; }
        public IPagedList<ServiceDocument> List { get; set; } 
    }
}