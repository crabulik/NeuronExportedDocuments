using System;
using System.Text;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Services.Logging
{
    public class LogUtility
    {
         public static string BuildExceptionMessage(Exception x)
         {
 
             Exception logException = x;
             if (x.InnerException != null)
             logException = x.InnerException;

             var strBuilder = new StringBuilder();
             strBuilder.AppendLine();
             strBuilder.Append(MainExceptionMessages.rs_ErrorInPath);
             strBuilder.AppendLine(System.Web.HttpContext.Current.Request.Path);
             // Get the QueryString along with the Virtual Path
             strBuilder.Append(MainExceptionMessages.rs_RawUrl);
             strBuilder.AppendLine(System.Web.HttpContext.Current.Request.RawUrl);
             // Get the error message
             strBuilder.Append(MainExceptionMessages.rs_Message);
             strBuilder.AppendLine(logException.Message);
             // Source of the message
             strBuilder.Append(MainExceptionMessages.rs_Source);
             strBuilder.AppendLine(logException.Source);
             // Stack Trace of the error
             strBuilder.Append(MainExceptionMessages.rs_StackTrace);
             strBuilder.AppendLine(logException.StackTrace);
             // Method where the error occurred
             strBuilder.Append(MainExceptionMessages.rs_TargetSite);
             strBuilder.AppendLine(logException.TargetSite.ToString());
 
             

             return strBuilder.ToString();
         }

         public static string BuildDocumentMessage(ServiceDocument doc)
        {
             var strBuilder = new StringBuilder();
             strBuilder.AppendLine();
             strBuilder.Append(MainExceptionMessages.rs_ServiceDocumentName);
             strBuilder.AppendLine(doc.Name);

             strBuilder.Append(MainExceptionMessages.rs_ServiceDocumenNeuronDbDocumentId);
             strBuilder.AppendLine(doc.NeuronDbDocumentId.ToString());

             strBuilder.Append(MainExceptionMessages.rs_ServiceDocumenStatus);
             strBuilder.AppendLine(doc.Status.ToString());
             return strBuilder.ToString();
        }
     }
    
}