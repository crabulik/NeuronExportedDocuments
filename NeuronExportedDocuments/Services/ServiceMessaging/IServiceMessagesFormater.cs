using System.Collections.Generic;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.ViewModels;
using NeuronExportedDocuments.Services.Config;

namespace NeuronExportedDocuments.Services.ServiceMessaging
{
    public interface IServiceMessagesFormater
    {
        string FormatByDocumentInfo(string inputMessage, ServiceDocumentInfo documentInfo);
        List<FormaterKey> GetDocumentInfoKeys();
        string FormatByGetDocumentViewModel(string inputMessage, GetDocumentViewModel documentInfoVM);
        List<FormaterKey> GetGetDocumentViewModelKeys();
        string FormatByServiceDocument(string inputMessage, ServiceDocument document);
        List<FormaterKey> GetServiceDocumentKeys();
        string FormatByGeneralSettings(string inputMessage, GeneralSettings settings);
        List<FormaterKey> GetGeneralSettingsKeys(); 
    }
}