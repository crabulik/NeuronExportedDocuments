using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNet.Identity;
using NeuronExportedDocuments.Models.Identity;
using NeuronExportedDocuments.Resources;
using PagedList;

namespace NeuronExportedDocuments.Models.ViewModels
{
    public class AccountsViewModel
    {
        private UserManager<ApplicationUser> UserManager { get; set; }

        public AccountsViewModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public AccountsFilter Filter { get; set; }

        public string CurrentUserId { get; set; }
        public IPagedList<ApplicationUser> List { get; set; }

        public string GetRoles(string userId)
        {
            var roleList = UserManager.GetRoles(userId);
            if (roleList.Count > 0)
            {
                var sb = new StringBuilder(roleList[0]);
                for (int i = 1; i < roleList.Count; i++)
                {
                    sb.Append(", ");
                    sb.Append(roleList[i]);
                }
                return sb.ToString();
            }
            return string.Empty;
        }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityEmail")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityCurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (IdentityResources), ErrorMessageResourceName = "rs_IdentityNewPassValidationLengthMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityNewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof (IdentityResources), ErrorMessageResourceName = "rs_IdentityConfirmPassValidationMessage")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityEmail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityPassword")]
        public string Password { get; set; }

        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityRememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityEmail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (IdentityResources), ErrorMessageResourceName = "rs_IdentityNewPassValidationLengthMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityPassword")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof (IdentityResources), ErrorMessageResourceName = "rs_IdentityConfirmPassCompareValidationMessage")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityEmail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (IdentityResources), ErrorMessageResourceName = "rs_IdentityNewPassValidationLengthMessage", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityPassword")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof (IdentityResources), ErrorMessageResourceName = "rs_IdentityConfirmPassCompareValidationMessage")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (IdentityResources), Name = "rs_IdentityEmail")]
        public string Email { get; set; }
    }
}