using AutoMapper;
using NeuronExportedDocuments.Interfaces;
using Ninject.Activation;

namespace NeuronExportedDocuments.Services.AutoMapper
{
    

    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public IMappingEngine Configure(IContext context)
        {
            Mapper.Initialize(x => x.AddProfile<ServiceDocumentProfile>());
            return Mapper.Engine;
        } 
    }
}