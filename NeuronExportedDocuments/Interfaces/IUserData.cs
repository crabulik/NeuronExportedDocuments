using System.Collections.Generic;
using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IUserData
    {

        Dictionary<string, ServiceDocumentInfo> GetCache { get; set; }
    }
}