using System;

namespace NeuronExportedDocuments.Models.Interfaces
{
    public interface IWebLogger
    {
        void Info(string message, ServiceDocument doc = null);

        void Warn(string message, ServiceDocument doc = null);

        void Debug(string message, ServiceDocument doc = null);

        void Error(string message, ServiceDocument doc = null);
        void Error(string message, Exception x, ServiceDocument doc = null);
        void Error(Exception x, ServiceDocument doc = null);

        void Fatal(string message);
        void Fatal(Exception x);
    }
}