using System.Collections.Generic;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IUserData
    {
        int FailTryCount { get; set; }
        Dictionary<string, ServiceDocumentInfo> GetCache { get; set; }
    }
}