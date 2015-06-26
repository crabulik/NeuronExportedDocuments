using System.Configuration;

namespace NeuronExportedDocuments.Services.Config
{
    public class GeneralSettings : ConfigurationSection
    {
        private const string DocumentAccessDaysCountPropertyName = "DocumentAccessDaysCount";
        private const string DocumentPassLengthPropertyName = "DocumentPassLength";
        private const string ServiceMessagesExperienceMinutesPropertyName = "ServiceMessagesExperienceMinutes";


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
    }
}