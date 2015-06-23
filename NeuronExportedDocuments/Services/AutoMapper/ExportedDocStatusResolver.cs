using System;
using AutoMapper;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Services.AutoMapper
{
    public class ExportedDocStatusResolver : ValueResolver<ExportedDocStatus, string>
    {
        protected override string ResolveCore(ExportedDocStatus source)
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
    }
}