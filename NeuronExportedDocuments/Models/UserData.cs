using System.Collections.Generic;
using NeuronExportedDocuments.Interfaces;

namespace NeuronExportedDocuments.Models
{
    public class UserData : IUserData
    {
        public Dictionary<string, ServiceDocumentInfo> GetCache { get; set; }

        public UserData()
        {
            GetCache = new Dictionary<string, ServiceDocumentInfo>();
        }
    }
}