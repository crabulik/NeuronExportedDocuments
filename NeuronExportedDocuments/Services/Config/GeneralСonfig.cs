using System.Configuration;
using NeuronExportedDocuments.Interfaces;

namespace NeuronExportedDocuments.Services.Config
{
    public class GeneralСonfig: IConfig
    {
        public GeneralSettings GeneralSettings
        {
            get
            {
                return (GeneralSettings)ConfigurationManager.GetSection("generalSettings");
            }
        }

        public MailSetting MailSetting
        {
            get
            {
                return (MailSetting)ConfigurationManager.GetSection("mailConfig");
            }
        }
    }
}