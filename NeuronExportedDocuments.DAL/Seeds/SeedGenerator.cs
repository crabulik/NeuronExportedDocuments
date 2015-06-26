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
            return result;
        }
    }
}