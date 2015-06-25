using System.Configuration;

namespace NeuronExportedDocuments.Services.Config
{
    public class GeneralSettings : ConfigurationSection
    {
        private const string DocumentAccessDaysCountPropertyName = "DocumentAccessDaysCount";
        private const string DocumentPassLengthPropertyName = "DocumentPassLength";


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
    }
}