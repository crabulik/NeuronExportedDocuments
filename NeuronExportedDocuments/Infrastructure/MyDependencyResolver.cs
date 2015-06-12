using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.DAL.Repositories;
using Ninject;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace NeuronExportedDocuments.Infrastructure
{
    public class MyDependencyResolver : IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        private IKernel _kernel;

        public MyDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;

            InitializeBindings();
        }

        private void InitializeBindings()
        {
            _kernel.Bind<WebDocumentConverter>().ToSelf().InSingletonScope();
            _kernel.Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}