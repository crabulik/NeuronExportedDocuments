using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using NeuronExportedDocuments.Infrastructure.Providers;
using NeuronExportedDocuments.Models.Identity;
using Owin;

[assembly: OwinStartup(typeof(NeuronExportedDocuments.Startup))]

namespace NeuronExportedDocuments
{
    public class Startup
    {
        private const int TokenExpiredTimeHours = 6;
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider {
                // Enables the application to validate the security stamp when the user logs in.
                // This is a security feature which is used when you change a password or add an external login to your account.  
                OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))}
 
            });

            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, DependencyResolver.Current),
                //AuthorizeEndpointPath = new PathString("/api/AccountApi/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(TokenExpiredTimeHours),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
