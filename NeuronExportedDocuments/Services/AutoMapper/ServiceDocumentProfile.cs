using AutoMapper;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Services.AutoMapper
{
    public class ServiceDocumentProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ServiceDocument, ServiceDocumentInfo>()
                .ForMember(vm => vm.StatusString, opt => opt.ResolveUsing<ExportedDocStatusResolver>()
                    .FromMember(p => p.Status));
        }

        public override string ProfileName
        {
            get { return this.GetType().Name; }
        } 
    }
}