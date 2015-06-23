using AutoMapper;
using Ninject.Activation;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IAutoMapperConfiguration
    {
        IMappingEngine Configure(IContext context);
    }
}