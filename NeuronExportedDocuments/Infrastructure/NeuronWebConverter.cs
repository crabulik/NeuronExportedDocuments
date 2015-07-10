using System;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Infrastructure
{
    public static class NeuronWebConverter
    {
        public static string ExportedDocStatusToSting(ExportedDocStatus source)
        {
            switch (source)
            {
                case ExportedDocStatus.Unhandled:
                    return StringResources.rs_ExportedDocStatusUnhandled;
                case ExportedDocStatus.Published:
                    return StringResources.rs_ExportedDocStatusPublished;
                case ExportedDocStatus.InfoSented:
                    return StringResources.rs_ExportedDocStatusInfoSented;
                case ExportedDocStatus.InArchive:
                    return StringResources.rs_ExportedDocStatusInArchive;
                default:
                    throw new ArgumentOutOfRangeException("source");
            }
        }

        public static string DocumentLogOperationTypeToSting(DocumentLogOperationType source)
        {
            switch (source)
            {
                case DocumentLogOperationType.Imported:
                    return StringResources.rs_DocumentLogOperationTypeImported;
                case DocumentLogOperationType.Published:
                    return StringResources.rs_DocumentLogOperationTypePublished;
                case DocumentLogOperationType.InfoSentedToUser:
                    return StringResources.rs_DocumentLogOperationTypeInfoSentedToUser;
                case DocumentLogOperationType.SuccessAccess:
                    return StringResources.rs_DocumentLogOperationTypeSuccessAccess;
                case DocumentLogOperationType.FailAccess:
                    return StringResources.rs_DocumentLogOperationTypeFailAccess;
                case DocumentLogOperationType.Blocked:
                    return StringResources.rs_DocumentLogOperationTypeBanned;
                case DocumentLogOperationType.SetToArchive:
                    return StringResources.rs_DocumentLogOperationTypeSetToArchive;
                case DocumentLogOperationType.UnblockedByAdmin:
                    return StringResources.rs_DocumentLogOperationTypeUnblockedByAdmin;
                case DocumentLogOperationType.Republished:
                    return StringResources.rs_DocumentLogOperationTypeRepublished;
                default:
                    throw new ArgumentOutOfRangeException("source");
            }
        }
    }
}