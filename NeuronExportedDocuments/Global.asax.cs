﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Services.Logging;
using NLog.Config;

namespace NeuronExportedDocuments
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(IUserData), DependencyResolver.Current.GetService<UserDataBinder>());

            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("utc_date", typeof(NeuronExportedDocuments.Services.Logging.NLog.UtcDateRenderer));
            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("web_variables", typeof(NeuronExportedDocuments.Services.Logging.NLog.WebVariablesRenderer));

            var binder = new DateTimeModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime), binder);
            ModelBinders.Binders.Add(typeof(DateTime?), binder);
        }

        protected void Application_Error()
        {
            Exception lastException = Server.GetLastError();
            var logger = DependencyResolver.Current.GetService<IWebLogger>();
            logger.Fatal(lastException);
        }
    }
}