using System.ComponentModel.DataAnnotations;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Models
{
    public class DocumentCredentials
    {
        [Display(Name = "rs_DocumentPublishId", ResourceType = typeof(MainMessages))]
        [Required(ErrorMessageResourceType = typeof(ValidateMessages), ErrorMessageResourceName = "rs_DocumentIdMustBeSet")]
        [StringLength(12, ErrorMessageResourceType = typeof(ValidateMessages), ErrorMessageResourceName = "rs_DocumentIdMustBe12")]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(ValidateMessages), ErrorMessageResourceName = "rs_DocumentIdOnlyDigits")]
        public string PublishId { get; set; }

        [Display(Name = "rs_DocumentPublishPassword", ResourceType = typeof(MainMessages))]
        [Required(ErrorMessageResourceType = typeof(ValidateMessages), ErrorMessageResourceName = "rs_DocumentPasswordMustBeSet")]
        [StringLength(8, ErrorMessageResourceType = typeof(ValidateMessages), ErrorMessageResourceName = "rs_DocumentPasswordMustBe8")]
        public string PublishPassword { get; set; } 
    }
}