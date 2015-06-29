using System.Collections.Generic;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.DAL.Seeds
{
    public static class SeedGenerator
    {
        public static List<ServiceMessage> ServiceMessageSeed()
        {
            var result = new List<ServiceMessage>(1);
            result.Add(new ServiceMessage
            {
                Key = ServiceMessageKey.SendCredentialsEmailMessage,
                IsDefault = true
            });
            result.Add(new ServiceMessage
            {
                Key = ServiceMessageKey.SendCredentialsEmailSubject,
                IsDefault = true
            });
            result.Add(new ServiceMessage
            {
                Key = ServiceMessageKey.HomeIndexHelloMessage,
                IsDefault = true
            });
            result.Add(new ServiceMessage
            {
                Key = ServiceMessageKey.HomeIndexHelloDescriptionMessage,
                IsDefault = true
            });
            result.Add(new ServiceMessage
            {
                Key = ServiceMessageKey.GetDocumentWarningMessage,
                IsDefault = true
            });
            return result;
        }
    }
}