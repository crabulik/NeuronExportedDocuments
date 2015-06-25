using System.Configuration;

namespace NeuronExportedDocuments.Services.Config
{
    public class MailSetting : ConfigurationSection
    {
        private const string SmtpServerPropertyName = "SmtpServer";
        private const string SmtpPortPropertyName = "SmtpPort";
        private const string SmtpUserNamePropertyName = "SmtpUserName";
        private const string SmtpPasswordPropertyName = "SmtpPassword";
        private const string SmtpReplyPropertyName = "SmtpReply";
        private const string SmtpUserPropertyName = "SmtpUser";
        private const string EnableSslPropertyName = "EnableSsl";


        [ConfigurationProperty(SmtpServerPropertyName, IsRequired = true)]
        public string SmtpServer
        {
            get
            {
                return this[SmtpServerPropertyName] as string;
            }
            set
            {
                this[SmtpServerPropertyName] = value;
            }
        }

        [ConfigurationProperty(SmtpPortPropertyName, IsRequired = false, DefaultValue = "25")]
        public int SmtpPort
        {
            get
            {
                return (int)this[SmtpPortPropertyName];
            }
            set
            {
                this[SmtpPortPropertyName] = value;
            }
        }

        [ConfigurationProperty(SmtpUserNamePropertyName, IsRequired = true)]
        public string SmtpUserName
        {
            get
            {
                return this[SmtpUserNamePropertyName] as string;
            }
            set
            {
                this[SmtpUserNamePropertyName] = value;
            }
        }

        [ConfigurationProperty(SmtpPasswordPropertyName, IsRequired = true)]
        public string SmtpPassword
        {
            get
            {
                return this[SmtpPasswordPropertyName] as string;
            }
            set
            {
                this[SmtpPasswordPropertyName] = value;
            }
        }

        [ConfigurationProperty(SmtpReplyPropertyName, IsRequired = true)]
        public string SmtpReply
        {
            get
            {
                return this[SmtpReplyPropertyName] as string;
            }
            set
            {
                this[SmtpReplyPropertyName] = value;
            }
        }

        [ConfigurationProperty(SmtpUserPropertyName, IsRequired = true)]
        public string SmtpUser
        {
            get
            {
                return this[SmtpUserPropertyName] as string;
            }
            set
            {
                this[SmtpUserPropertyName] = value;
            }
        }

        [ConfigurationProperty(EnableSslPropertyName, IsRequired = false, DefaultValue = "false")]
        public bool EnableSsl
        {
            get
            {
                return (bool)this[EnableSslPropertyName];
            }
            set
            {
                this[EnableSslPropertyName] = value;
            }
        }
    }
}