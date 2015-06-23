﻿using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using AutoMapper;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.DAL.Repositories;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Services.AutoMapper;
using NeuronExportedDocuments.Services.Logging;
using NeuronExportedDocuments.Services.Logging.NLog;
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
            _kernel.Bind<IWebDocumentProcessor>().To<WebDocumentProcessor>().InSingletonScope();
            _kernel.Bind<IWebLogger>().To<NLogLogger>().InSingletonScope();
            _kernel.Bind<IUserData>().To<UserData>();
            _kernel.Bind<UserDataBinder>().ToSelf();

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