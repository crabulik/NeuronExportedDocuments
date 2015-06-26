using System.Collections.Generic;
using System.Text;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Services.ServiceMessaging
{
    public class ServiceMessagesFormater
    {
        private const string ServiceDocumentCreatDateKey = "[@DOCINFO_CREATEDATE]";
        private const string ServiceDocumentOpenDateKey = "[@DOCINFO_OPENDATE]";
        private const string ServiceDocumentStatusKey = "[@DOCINFO_STATUS]";

        public string FormatByDocumentInfo(string inputMessage, ServiceDocumentInfo documentInfo)
        {
            var sb = new StringBuilder(inputMessage);

            FormatByDocumentInfo(sb, documentInfo);

            return sb.ToString();
        }

        private void FormatByDocumentInfo(StringBuilder sb, ServiceDocumentInfo documentInfo)
        {
            sb.Replace(ServiceDocumentCreatDateKey,
                documentInfo.CreatDate.ToShortDateString() + " " + documentInfo.CreatDate.ToLongTimeString());
            sb.Replace(ServiceDocumentCreatDateKey,
                (documentInfo.OpenDate.HasValue)
                    ? documentInfo.CreatDate.ToShortDateString() + " " + documentInfo.CreatDate.ToLongTimeString()
                    : ServiceMessagesResources.rs_NotSet);
            sb.Replace(ServiceDocumentStatusKey,
                documentInfo.StatusString);
        }

        public List<FormaterKey> GetDocumentInfoKeys()
        {
            var result = new List<FormaterKey>();
            result.Add(new FormaterKey(ServiceDocumentCreatDateKey, ServiceMessagesResources.rs_ServiceDocumentCreatDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentOpenDateKey, ServiceMessagesResources.rs_ServiceDocumentOpenDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentStatusKey, ServiceMessagesResources.rs_ServiceDocumentStatusKeyInfo));
            return result;
        }

        public string FormatByServiceDocument(string inputMessage, ServiceDocument document)
        {
            var sb = new StringBuilder(inputMessage);

            FormatByDocumentInfo(sb, documentInfo);

            return sb.ToString();
        }

        private void FormatByServiceDocument(StringBuilder sb, ServiceDocumentInfo document)
        {
            sb.Replace(ServiceDocumentCreatDateKey,
                documentInfo.CreatDate.ToShortDateString() + " " + documentInfo.CreatDate.ToLongTimeString());
            sb.Replace(ServiceDocumentCreatDateKey,
                (documentInfo.OpenDate.HasValue)
                    ? documentInfo.CreatDate.ToShortDateString() + " " + documentInfo.CreatDate.ToLongTimeString()
                    : ServiceMessagesResources.rs_NotSet);
            sb.Replace(ServiceDocumentStatusKey,
                documentInfo.StatusString);
        }

        public List<FormaterKey> GetServiceDocumentKeys()
        {
            var result = new List<FormaterKey>();
            result.Add(new FormaterKey(ServiceDocumentCreatDateKey, ServiceMessagesResources.rs_ServiceDocumentCreatDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentOpenDateKey, ServiceMessagesResources.rs_ServiceDocumentOpenDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentStatusKey, ServiceMessagesResources.rs_ServiceDocumentStatusKeyInfo));
            return result;
        }
    }
}