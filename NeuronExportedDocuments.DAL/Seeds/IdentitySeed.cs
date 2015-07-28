using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NeuronExportedDocuments.Models.Identity;

namespace NeuronExportedDocuments.DAL.Seeds
{
    public class IdentitySeed
    {
        public void InitializeIdentityForEf(DocumentContext context)
        {
            #region Role Seed

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var role = roleManager.FindByName(IdentityConstants.AdminRole);
            if (role == null)
            {
                role = new IdentityRole
                {
                    Name = IdentityConstants.AdminRole
                };
                roleManager.Create(role);
            }

            role = roleManager.FindByName(IdentityConstants.SyncServiceRole);
            if (role == null)
            {
                role = new IdentityRole
                {
                    Name = IdentityConstants.SyncServiceRole
                };
                roleManager.Create(role);
            }

            #endregion




            #region User Seed

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var user = userManager.FindByName(IdentityConstants.DefaultAdminName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = IdentityConstants.DefaultAdminMail, Email = IdentityConstants.DefaultAdminMail };
                userManager.Create(user, IdentityConstants.DefaultAdminPass);
                userManager.SetLockoutEnabled(user.Id, false);
                userManager.AddToRole(user.Id, IdentityConstants.AdminRole);
            }

            user = userManager.FindByName(IdentityConstants.DefaultSyncServiceName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = IdentityConstants.DefaultSyncServiceMail, Email = IdentityConstants.DefaultSyncServiceMail };
                userManager.Create(user, IdentityConstants.DefaultSyncServicePass);
                userManager.SetLockoutEnabled(user.Id, false);
                userManager.AddToRole(user.Id, IdentityConstants.SyncServiceRole);
            }
            #endregion

        }
    }
}