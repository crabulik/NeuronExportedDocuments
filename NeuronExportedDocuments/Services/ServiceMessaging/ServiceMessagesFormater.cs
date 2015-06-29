using System.Collections.Generic;
using System.Text;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.ViewModels;
using NeuronExportedDocuments.Resources;
using NeuronExportedDocuments.Services.Config;

namespace NeuronExportedDocuments.Services.ServiceMessaging
{

    public class ServiceMessagesFormater : IServiceMessagesFormater
    {
        private const string ServiceDocumentInfoCreatDateKey = "[@DOCINFO_CREATEDATE]";
        private const string ServiceDocumentInfoOpenDateKey = "[@DOCINFO_OPENDATE]";
        private const string ServiceDocumentInfoStatusKey = "[@DOCINFO_STATUS]";
        private const string ServiceDocumentInfoPublishIdKey = "[@DOCINFO_PUBLISHID]";
        private const string ServiceDocumentAccessDaysCountKey = "[@DOCINFO_ACCESSDAYSCOUNT]";
        private const string ServiceDocumentAccessFinalDateKey = "[@DOCINFO_ACCESSFINALDATE]";

        private const string ServiceDocumentCreatDateKey = "[@DOC_CREATEDATE]";
        private const string ServiceDocumentOpenDateKey = "[@DOC_OPENDATE]";
        private const string ServiceDocumentStatusKey = "[@DOC_STATUS]";
        private const string ServiceDocumentPublishIdKey = "[@DOC_PUBLISHID]";
        private const string ServiceDocumentPublishPasswordKey = "[@DOC_PUBLISHPASSWORD]";
        private const string ServiceDocumentNameKey = "[@DOC_NAME]";
        private const string ServiceDocumentDeliveryEmailKey = "[@DOC_DELIVERYEMAIL]";
        private const string ServiceDocumentDeliveryPhoneKey = "[@DOC_DELIVERYPHONE]";

        private const string GeneralSettingsMainSiteUrlKey = "[@SETT_MAINSITEURL]";
        private const string GeneralSettingsGetDocumentUrlKey = "[@SETT_GETDOCUMENTURL]";
        private const string GeneralSupportEmailKey = "[@SETT_SUPPORTEMAIL]";

        public ServiceMessagesFormater()
        {

        }

        #region ServiceDocumentInfo

        public string FormatByDocumentInfo(string inputMessage, ServiceDocumentInfo documentInfo)
        {
            var sb = new StringBuilder(inputMessage);

            FormatByDocumentInfo(sb, documentInfo);

            return sb.ToString();
        }

        private void FormatByDocumentInfo(StringBuilder sb, ServiceDocumentInfo documentInfo)
        {
            sb.Replace(ServiceDocumentInfoCreatDateKey,
                documentInfo.CreatDate.ToShortDateString() + " " + documentInfo.CreatDate.ToLongTimeString());
            sb.Replace(ServiceDocumentInfoOpenDateKey,
                (documentInfo.OpenDate.HasValue)
                    ? documentInfo.OpenDate.Value.ToShortDateString() + " " + documentInfo.OpenDate.Value.ToLongTimeString()
                    : ServiceMessagesResources.rs_NotSet);
            sb.Replace(ServiceDocumentInfoStatusKey,
                documentInfo.StatusString);
            sb.Replace(ServiceDocumentInfoPublishIdKey,
                documentInfo.PublishId);
        }

        public List<FormaterKey> GetDocumentInfoKeys()
        {
            var result = new List<FormaterKey>();
            result.Add(new FormaterKey(ServiceDocumentInfoCreatDateKey,
                ServiceMessagesResources.rs_ServiceDocumentInfoCreatDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentInfoOpenDateKey,
                ServiceMessagesResources.rs_ServiceDocumentInfoOpenDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentInfoStatusKey,
                ServiceMessagesResources.rs_ServiceDocumentInfoStatusKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentInfoPublishIdKey,
                ServiceMessagesResources.rs_ServiceDocumentInfoPublishIdKeyInfo));
            return result;
        }
        public string FormatByGetDocumentViewModel(string inputMessage, GetDocumentViewModel documentInfoVM)
        {
            var sb = new StringBuilder(inputMessage);

            FormatByGetDocumentViewModel(sb, documentInfoVM);

            return sb.ToString();
        }

        private void FormatByGetDocumentViewModel(StringBuilder sb, GetDocumentViewModel documentInfoVM)
        {
            FormatByDocumentInfo(sb, documentInfoVM.ServiceDocumentInfo);

            sb.Replace(ServiceDocumentAccessDaysCountKey,
                documentInfoVM.AccessDaysCount.ToString());
            sb.Replace(ServiceDocumentAccessFinalDateKey,
                documentInfoVM.AccessFinalDate);
        }

        public List<FormaterKey> GetGetDocumentViewModelKeys()
        {
            var result = GetDocumentInfoKeys();
            result.Add(new FormaterKey(ServiceDocumentAccessDaysCountKey,
                ServiceMessagesResources.rs_ServiceDocumentAccessDaysCountKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentAccessFinalDateKey,
                ServiceMessagesResources.rs_ServiceDocumentAccessFinalDateKeyInfo));

            return result;
        }

        #endregion


        #region ServiceDocument

        public string FormatByServiceDocument(string inputMessage, ServiceDocument document)
        {
            var sb = new StringBuilder(inputMessage);

            FormatByServiceDocument(sb, document);

            return sb.ToString();
        }

        private void FormatByServiceDocument(StringBuilder sb, ServiceDocument document)
        {
            sb.Replace(ServiceDocumentCreatDateKey,
                document.CreatDate.ToShortDateString() + " " + document.CreatDate.ToLongTimeString());
            sb.Replace(ServiceDocumentOpenDateKey,
                (document.OpenDate.HasValue)
                    ? document.OpenDate.Value.ToShortDateString() + " " + document.OpenDate.Value.ToLongTimeString()
                    : ServiceMessagesResources.rs_NotSet);
            sb.Replace(ServiceDocumentStatusKey, NeuronWebConverter.ExportedDocStatusToSting(document.Status));
            sb.Replace(ServiceDocumentPublishIdKey, document.PublishId);
            sb.Replace(ServiceDocumentPublishPasswordKey, document.PublishPassword);
            sb.Replace(ServiceDocumentNameKey, document.Name);
            sb.Replace(ServiceDocumentDeliveryEmailKey, document.DeliveryEMail);
            sb.Replace(ServiceDocumentDeliveryPhoneKey, document.DeliveryPhone);
        }

        public List<FormaterKey> GetServiceDocumentKeys()
        {
            var result = new List<FormaterKey>();
            result.Add(new FormaterKey(ServiceDocumentCreatDateKey,
                ServiceMessagesResources.rs_ServiceDocumentCreatDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentOpenDateKey,
                ServiceMessagesResources.rs_ServiceDocumentOpenDateKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentStatusKey,
                ServiceMessagesResources.rs_ServiceDocumentStatusKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentPublishIdKey,
                ServiceMessagesResources.rs_ServiceDocumentPublishIdKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentPublishPasswordKey,
                ServiceMessagesResources.rs_ServiceDocumentPublishPasswordKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentNameKey,
                ServiceMessagesResources.rs_ServiceDocumentNameKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentDeliveryEmailKey,
                ServiceMessagesResources.rs_ServiceDocumentDeliveryEmailKeyInfo));
            result.Add(new FormaterKey(ServiceDocumentDeliveryPhoneKey,
                ServiceMessagesResources.rs_ServiceDocumentDeliveryPhoneKeyInfo));
            return result;
        }

        #endregion

        #region GeneralSettings

        public string FormatByGeneralSettings(string inputMessage, GeneralSettings settings)
        {
            var sb = new StringBuilder(inputMessage);

            FormatByGeneralSettings(sb, settings);

            return sb.ToString();
        }

        private void FormatByGeneralSettings(StringBuilder sb, GeneralSettings settings)
        {

            sb.Replace(GeneralSettingsMainSiteUrlKey, settings.MainSiteUrl);
            sb.Replace(GeneralSettingsGetDocumentUrlKey, settings.GetDocumentUrl);
            sb.Replace(GeneralSupportEmailKey, settings.SupportEmail);
        }

        public List<FormaterKey> GetGeneralSettingsKeys()
        {
            var result = new List<FormaterKey>();
            result.Add(new FormaterKey(GeneralSettingsMainSiteUrlKey,
                ServiceMessagesResources.rs_GeneralSettingsMainSiteUrlKeyInfo));
            result.Add(new FormaterKey(GeneralSettingsGetDocumentUrlKey,
                ServiceMessagesResources.rs_GeneralSettingsGetDocumentUrlKeyInfo));
            result.Add(new FormaterKey(GeneralSupportEmailKey,
                ServiceMessagesResources.rs_GeneralSupportEmailKeyInfo));

            return result;
        }

        #endregion

    }
}