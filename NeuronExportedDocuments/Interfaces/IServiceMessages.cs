using System.Collections.Generic;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.ViewModels;
using NeuronExportedDocuments.Services.Config;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IServiceMessages
    {
        string GetMessage(ServiceMessageKey key);
        void UpdateCachedMessage(ServiceMessage message);
        string GetDefaultKeyDisplayName(ServiceMessageKey key);
        string GetDefaultMessage(ServiceMessageKey key);
        List<FormaterKey> GetFormatKeys(ServiceMessageKey key);
        string FormatMessageByDocumentInfo(string inputMessage, ServiceDocumentInfo documentInfo);
        string FormatMessageByDocumentInfo(ServiceMessageKey key, ServiceDocumentInfo documentInfo);
        List<FormaterKey> GetDocumentInfoKeys();
        string FormatMessageByGetDocumentViewModel(string inputMessage, GetDocumentViewModel documentInfoVM);
        string FormatMessageByGetDocumentViewModel(ServiceMessageKey key, GetDocumentViewModel documentInfoVM);
        List<FormaterKey> GetGetDocumentViewModelKeys();
        string FormatMessageByServiceDocument(string inputMessage, ServiceDocument document);
        string FormatMessageByServiceDocument(ServiceMessageKey key, ServiceDocument document);
        List<FormaterKey> GetServiceDocumentKeys();
        List<FormaterKey> GetGeneralSettingsKeys();
    }
}