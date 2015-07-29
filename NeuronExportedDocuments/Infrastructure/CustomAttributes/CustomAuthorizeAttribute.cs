using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;

namespace NeuronExportedDocuments.Infrastructure.CustomAttributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(filterContext);
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "AccessDenied", backAction = filterContext.ActionDescriptor.ActionName, 
                        backController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName }));
                filterContext.HttpContext.Response.StatusCode = 403;
            }
        }
    }
}