using System;
using System.Linq;
using System.Runtime.Caching;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Services.ServiceMessaging
{
    public class ServiceMessages
    {
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }

        private MemoryCache Cache { get; set; }

        private IConfig Config { get; set; }
        public ServiceMessages(IDBUnitOfWork uow, IWebLogger logger, IConfig config)
        {
            Database = uow;
            Log = logger;
            Config = config;
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
                default:
                    throw new ArgumentOutOfRangeException("key");
            }
        }

    }
}