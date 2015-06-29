using AutoMapper;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Services.AutoMapper
{
    public class ExportedDocStatusResolver : ValueResolver<ExportedDocStatus, string>
    {
        protected override string ResolveCore(ExportedDocStatus source)
        {
            return NeuronWebConverter.ExportedDocStatusToSting(source);
        }
    }
}