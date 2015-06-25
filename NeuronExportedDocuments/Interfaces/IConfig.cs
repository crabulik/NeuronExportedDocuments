using NeuronExportedDocuments.Services.Config;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IConfig
    {
        GeneralSettings GeneralSettings { get; }

        MailSetting MailSetting { get; }
    }
}