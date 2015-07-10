using System.Collections.Generic;
using NeuronExportedDocuments.Interfaces;

namespace NeuronExportedDocuments.Models
{
    public class UserData : IUserData
    {
        public int FailTryCount { get; set; }
        public Dictionary<string, ServiceDocumentInfo> GetCache { get; set; }

        public UserData()
        {
            FailTryCount = 0;
            GetCache = new Dictionary<string, ServiceDocumentInfo>();
        }
    }
}