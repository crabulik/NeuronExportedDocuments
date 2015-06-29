using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Models.ViewModels;
using NeuronExportedDocuments.Resources;
using NeuronExportedDocuments.Services.Config;

namespace NeuronExportedDocuments.Services.ServiceMessaging
{
    public class ServiceMessages : IServiceMessages
    {
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }

        private MemoryCache Cache { get; set; }

        private IConfig Config { get; set; }
        private IServiceMessagesFormater Formatter { get; set; }
        public ServiceMessages(IDBUnitOfWork uow, IWebLogger logger, IConfig config, IServiceMessagesFormater formatter)
        {
            Database = uow;
            Log = logger;
            Config = config;
            Formatter = formatter;
            Cache = MemoryCache.Default;
        }
        public string GetMessage(ServiceMessageKey key)
        {
            var defServiceMessage = new ServiceMessage
            {
                Key = key,
                IsDefault = true
            };
            var cached = Cache[defServiceMessage.GetCachedKey()];
            if (cached != null)
            {
                defServiceMessage = ((ServiceMessage) cached);               
            }
            else
            {
                var dbMessage = Database.ServiceMessages.Find(p => p.Key == key).FirstOrDefault();
                if (dbMessage != null)
                    defServiceMessage = dbMessage;
                if (defServiceMessage.IsDefault)
                {
                    defServiceMessage.Message = GetDefaultMessage(defServiceMessage.Key);
                    defServiceMessage.IsDefault = false;
                }
                // General settings keys are pushed to the cache
                defServiceMessage.Message = Formatter.FormatByGeneralSettings(defServiceMessage.Message,
                    Config.GeneralSettings);
                
                Cache.Add(defServiceMessage.GetCachedKey(), defServiceMessage,
                    DateTimeOffset.Now.AddMinutes(Config.GeneralSettings.ServiceMessagesExperienceMinutes));

            }

            return defServiceMessage.Message;
        }

        public string GetDefaultMessage(ServiceMessageKey key)
        {
            switch (key)
            {
                case ServiceMessageKey.SendCredentialsEmailMessage:
                    return ServiceMessagesResources.rs_DefSendCredentialsEmailMessage;
                case ServiceMessageKey.SendCredentialsEmailSubject:
                    return ServiceMessagesResources.rs_DefSendCredentialsEmailSubject;
                case ServiceMessageKey.HomeIndexHelloMessage:
                    return ServiceMessagesResources.rs_DefHomeIndexHelloMessage;
                case ServiceMessageKey.HomeIndexHelloDescriptionMessage:
                    return ServiceMessagesResources.rs_DefHomeIndexHelloDescriptionMessage;
                case ServiceMessageKey.GetDocumentWarningMessage:
                    return ServiceMessagesResources.rs_DefGetDocumentWarningMessage;
                default:
                    throw new ArgumentOutOfRangeException("key");
            }
        }

        #region ServiceDocumentInfo

        public string FormatMessageByDocumentInfo(string inputMessage, ServiceDocumentInfo documentInfo)
        {
            return Formatter.FormatByDocumentInfo(inputMessage, documentInfo);
        }

        public string FormatMessageByDocumentInfo(ServiceMessageKey key, ServiceDocumentInfo documentInfo)
        {
            return FormatMessageByDocumentInfo(GetMessage(key), documentInfo);
        }

        public List<FormaterKey> GetDocumentInfoKeys()
        {
            return Formatter.GetDocumentInfoKeys();
        }

        #endregion


        #region GetDocumentViewModel

        public string FormatMessageByGetDocumentViewModel(string inputMessage, GetDocumentViewModel documentInfoVM)
        {
            return Formatter.FormatByGetDocumentViewModel(inputMessage, documentInfoVM);
        }

        public string FormatMessageByGetDocumentViewModel(ServiceMessageKey key, GetDocumentViewModel documentInfoVM)
        {
            return FormatMessageByGetDocumentViewModel(GetMessage(key), documentInfoVM);
        }

        public List<FormaterKey> GetGetDocumentViewModelKeys()
        {
            return Formatter.GetGetDocumentViewModelKeys();
        }

        #endregion


        #region ServiceDocument

        public string FormatMessageByServiceDocument(string inputMessage, ServiceDocument document)
        {
            return Formatter.FormatByServiceDocument(inputMessage, document);
        }

        public string FormatMessageByServiceDocument(ServiceMessageKey key, ServiceDocument document)
        {
            return FormatMessageByServiceDocument(GetMessage(key), document);
        }

        public List<FormaterKey> GetServiceDocumentKeys()
        {
            return Formatter.GetServiceDocumentKeys();
        }

        #endregion


        #region GeneralSettings

        public List<FormaterKey> GetGeneralSettingsKeys()
        {
            return Formatter.GetGeneralSettingsKeys();
        }

        #endregion

    }
}