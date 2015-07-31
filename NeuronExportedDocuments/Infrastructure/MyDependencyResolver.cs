using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NeuronDocumentSync.Cypher;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.DAL.Repositories;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Identity;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Services.AutoMapper;
using NeuronExportedDocuments.Services.Config;
using Ninject.Web.Common;
using NeuronExportedDocuments.Services.Logging.DocumentOperation;
using NeuronExportedDocuments.Services.Logging.NLog;
using NeuronExportedDocuments.Services.ServiceMessaging;
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
            string publicKey;
            var cyph = new RSACypher();
            cyph.CreateKeys(out publicKey);
            _kernel.Bind<WebDocumentConverter>().ToSelf().InRequestScope();
            _kernel.Bind<IDBUnitOfWork>().To<EFUnitOfWork>();
            _kernel.Bind<IWebDocumentProcessor>().To<WebDocumentProcessor>().InRequestScope();
            _kernel.Bind<IWebLogger>().To<NLogLogger>().InRequestScope();
            _kernel.Bind<IDocumentOperationLogger>().To<DocumentOperationLogger>().InRequestScope();
            _kernel.Bind<IConfig>().To<GeneralСonfig>().InRequestScope();
            _kernel.Bind<IServiceMessagesFormater>().To<ServiceMessagesFormater>().InRequestScope();
            _kernel.Bind<IUserData>().To<UserData>();
            _kernel.Bind<IServiceMessages>().To<ServiceMessages>();
            _kernel.Bind<UserDataBinder>().ToSelf();
            _kernel.Bind<UserManager<ApplicationUser>>().To<ApplicationUserManager>().InRequestScope();
            _kernel.Bind<RoleManager<IdentityRole>>().To<ApplicationRoleManager>().InRequestScope();
            _kernel.Bind<EmailService>().ToSelf().InRequestScope();
            _kernel.Bind<IRSACypher>().To<RSACypher>();

            IAutoMapperConfiguration conf = new AutoMapperConfiguration();
            _kernel.Bind<IMappingEngine>().ToMethod(conf.Configure);
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