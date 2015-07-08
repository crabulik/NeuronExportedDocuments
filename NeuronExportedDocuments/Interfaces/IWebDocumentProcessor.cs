﻿using NeuronExportedDocuments.Models;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IWebDocumentProcessor
    {
        bool SendEmail(ServiceDocument doc, string subject, string body);
        bool SendDocInfo(ServiceDocument doc);
        bool TrySetPublishId(ServiceDocument doc);
        string GetPass(int x = 8);
        bool PublishDocument(ServiceDocument doc);

        bool RepublishDocument(ServiceDocument doc);
    }
}