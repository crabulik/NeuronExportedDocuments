using System.Configuration;
using NeuronExportedDocuments.Interfaces;

namespace NeuronExportedDocuments.Services.Config
{
    public class GeneralСonfig: IConfig
    {
        public MailSetting MailSetting
        {
            get
            {
                return (MailSetting)ConfigurationManager.GetSection("mailConfig");
            }
        }
    }
}