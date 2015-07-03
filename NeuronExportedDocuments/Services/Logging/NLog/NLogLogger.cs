using System;
using System.Collections.Generic;
using NeuronExportedDocuments.Models;
using NLog;
using NeuronExportedDocuments.Models.Interfaces;

namespace NeuronExportedDocuments.Services.Logging.NLog
{
    public class NLogLogger : IWebLogger
    {
        private const string TraceLogLevelName = "Trace";
        private const string DebugLogLevelName = "Debug";
        private const string InfoLogLevelName = "Info";
        private const string WarnLogLevelName = "Warn";
        private const string ErrorLogLevelName = "Error";
        private const string FatalLogLevelName = "Fatal";
         private Logger _logger;
 
         public NLogLogger()
         {
            _logger = LogManager.GetCurrentClassLogger();
         }

         public void Info(string message, ServiceDocument doc = null)
         {
             if (doc != null)
                 message += LogUtility.BuildDocumentMessage(doc);
            _logger.Info(message);
         }

         public void Warn(string message, ServiceDocument doc = null)
         {
             if (doc != null)
                 message += LogUtility.BuildDocumentMessage(doc);
             _logger.Warn(message);
         }

         public void Debug(string message, ServiceDocument doc = null)
         {
             if (doc != null)
                 message += LogUtility.BuildDocumentMessage(doc);
             _logger.Debug(message);
         }

         public void Error(string message, ServiceDocument doc = null)
         {
             if (doc != null)
                 message += LogUtility.BuildDocumentMessage(doc);
            _logger.Error(message);
         }

         public void Error(Exception x, ServiceDocument doc = null)
         {
             Error(LogUtility.BuildExceptionMessage(x), doc);
         }

         public void Error(string message, Exception x, ServiceDocument doc = null)
         {
             Error(message + Environment.NewLine + LogUtility.BuildExceptionMessage(x), doc);
         }
 
         public void Fatal(string message)
         {
            _logger.Fatal(message);
         }
 
         public void Fatal(Exception x)
         {
            Fatal(LogUtility.BuildExceptionMessage(x));
         }

         public List<string> GetAllLogLevels()
        {
            var result = new List<string>();
            result.Add(TraceLogLevelName);
            result.Add(DebugLogLevelName);
            result.Add(InfoLogLevelName);
            result.Add(WarnLogLevelName);
            result.Add(ErrorLogLevelName);
            result.Add(FatalLogLevelName);
            return result;
        }
     }
         
}