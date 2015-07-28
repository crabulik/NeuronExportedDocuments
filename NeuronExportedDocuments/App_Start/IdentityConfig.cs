using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Infrastructure;
using NeuronExportedDocuments.Models.Identity;
using NeuronExportedDocuments.Resources;
using Ninject;

namespace NeuronExportedDocuments
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IDBUnitOfWork database, EmailService emailService)
            : base(new UserStore<ApplicationUser>(database.GetIdentityDbContext()))
        {

            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            EmailService = emailService;
            var provider = new DpapiDataProtectionProvider("NeuronExportedDocuments");

            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                provider.Create("EmailConfirmation"));
        }

        /*[Inject]
        public void InitProtection(IdentityFactoryOptions<ApplicationUserManager> options)
        {
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = StringResources.rs_InitProtectionPhoneCode
            });
            RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = StringResources.rs_InitProtectionEmailCodeSubject,
                BodyFormat = StringResources.rs_InitProtectionEmailCodeBodyFormat
            });
            EmailService = new EmailService();
            SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }*/
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IDBUnitOfWork database) :
            base(new RoleStore<IdentityRole>(database.GetIdentityDbContext()))
        {
            
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}