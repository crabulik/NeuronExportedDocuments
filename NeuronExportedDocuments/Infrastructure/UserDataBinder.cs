using System.Web.Mvc;
using NeuronExportedDocuments.Interfaces;
using Ninject;

namespace NeuronExportedDocuments.Infrastructure
{
    
    public class UserDataBinder: IModelBinder
    {
        private IKernel _kernel;
        private const string SessionKey = "UserData";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            IUserData userData = null;
            if (controllerContext.HttpContext.Session != null)
            {
                userData = (IUserData)controllerContext.HttpContext.Session[SessionKey];
            }

            if (userData == null)
            {
                userData = _kernel.Get<IUserData>();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[SessionKey] = userData;
                }
            }
            return userData;
        }

        public UserDataBinder(IKernel ninjectKernel)
        {
            _kernel = ninjectKernel;
        }
    }
}