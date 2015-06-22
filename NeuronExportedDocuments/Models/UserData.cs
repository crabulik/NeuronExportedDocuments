using System.Collections.Generic;
using NeuronExportedDocuments.Interfaces;

namespace NeuronExportedDocuments.Models
{
    public class UserData : IUserData
    {
        public Dictionary<string, ServiceDocument> GetCache { get; set; }

        public UserData()
        {
            GetCache = new Dictionary<string, ServiceDocument>();
        }
    }
}