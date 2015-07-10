using System.Configuration;

namespace NeuronExportedDocuments.Services.Config
{
    public class GeneralSettings : ConfigurationSection
    {
        private const string DocumentAccessDaysCountPropertyName = "DocumentAccessDaysCount";
        private const string DocumentPassLengthPropertyName = "DocumentPassLength";
        private const string ServiceMessagesExperienceMinutesPropertyName = "ServiceMessagesExperienceMinutes";
        private const string MainSiteUrlPropertyName = "MainSiteUrl";
        private const string GetDocumentUrlPropertyName = "GetDocumentUrl";
        private const string SupportEmailPropertyName = "SupportEmail";

        private const string RecaptchaSiteKeyName = "RecaptchaSiteKey";
        private const string RecaptchaSecretKeyName = "RecaptchaSecretKey";
        private const string FailSiteAccessForCaptchaName = "FailSiteAccessForCaptcha";
        private const string FailDocumentAccessForCaptchaName = "FailDocumentAccessForCaptcha";
        private const string FailAccessCountForBanName = "FailAccessCountForBan";


        [ConfigurationProperty(DocumentAccessDaysCountPropertyName, IsRequired = false, DefaultValue = "31")]
        public int DocumentAccessDaysCount
        {
            get
            {
                return (int)this[DocumentAccessDaysCountPropertyName];
            }
            set
            {
                this[DocumentAccessDaysCountPropertyName] = value;
            }
        }

        [ConfigurationProperty(DocumentPassLengthPropertyName, IsRequired = false, DefaultValue = "8")]
        public int DocumentPassLength
        {
            get
            {
                return (int)this[DocumentPassLengthPropertyName];
            }
            set
            {
                if (value < 4)
                    this[DocumentPassLengthPropertyName] = 4;
                else if (value  > 16)
                {
                    this[DocumentPassLengthPropertyName] = 8;
                }
                else
                {
                    this[DocumentPassLengthPropertyName] = value;
                }
                    
            }
        }

        [ConfigurationProperty(ServiceMessagesExperienceMinutesPropertyName, IsRequired = false, DefaultValue = "60")]
        public int ServiceMessagesExperienceMinutes
        {
            get
            {
                return (int)this[ServiceMessagesExperienceMinutesPropertyName];
            }
            set
            {
                if (value < 1)
                    this[ServiceMessagesExperienceMinutesPropertyName] = 4;
                else
                {
                    this[ServiceMessagesExperienceMinutesPropertyName] = value;
                }

            }
        }

        [ConfigurationProperty(MainSiteUrlPropertyName, IsRequired = true)]
        public string MainSiteUrl
        {
            get
            {
                return (string)this[MainSiteUrlPropertyName];
            }
            set
            {
                this[MainSiteUrlPropertyName] = value;
            }
        }

        [ConfigurationProperty(GetDocumentUrlPropertyName, IsRequired = true)]
        public string GetDocumentUrl
        {
            get
            {
                return (string)this[GetDocumentUrlPropertyName];
            }
            set
            {
                this[GetDocumentUrlPropertyName] = value;
            }
        }

        [ConfigurationProperty(SupportEmailPropertyName, IsRequired = true)]
        public string SupportEmail
        {
            get
            {
                return (string)this[SupportEmailPropertyName];
            }
            set
            {
                this[SupportEmailPropertyName] = value;
            }
        }

        [ConfigurationProperty(RecaptchaSiteKeyName, IsRequired = true)]
        public string RecaptchaSiteKey
        {
            get
            {
                return (string)this[RecaptchaSiteKeyName];
            }
            set
            {
                this[RecaptchaSiteKeyName] = value;
            }
        }

        [ConfigurationProperty(RecaptchaSecretKeyName, IsRequired = true)]
        public string RecaptchaSecretKey
        {
            get
            {
                return (string)this[RecaptchaSecretKeyName];
            }
            set
            {
                this[RecaptchaSecretKeyName] = value;
            }
        }

        [ConfigurationProperty(FailSiteAccessForCaptchaName, IsRequired = false, DefaultValue = "3")]
        public int FailSiteAccessForCaptcha
        {
            get
            {
                return (int)this[FailSiteAccessForCaptchaName];
            }
            set
            {
                if (value < 1)
                    this[FailSiteAccessForCaptchaName] = 3;
                else
                {
                    this[FailSiteAccessForCaptchaName] = value;
                }

            }
        }

        [ConfigurationProperty(FailDocumentAccessForCaptchaName, IsRequired = false, DefaultValue = "3")]
        public int FailDocumentAccessForCaptcha
        {
            get
            {
                return (int)this[FailDocumentAccessForCaptchaName];
            }
            set
            {
                if (value < 1)
                    this[FailDocumentAccessForCaptchaName] = 3;
                else
                {
                    this[FailDocumentAccessForCaptchaName] = value;
                }

            }
        }

        [ConfigurationProperty(FailAccessCountForBanName, IsRequired = false, DefaultValue = "8")]
        public int FailAccessCountForBan
        {
            get
            {
                return (int)this[FailAccessCountForBanName];
            }
            set
            {
                if (value < 1)
                    this[FailAccessCountForBanName] = 8;
                else
                {
                    this[FailAccessCountForBanName] = value;
                }

            }
        }
    }
}