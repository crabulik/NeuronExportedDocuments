using NeuronExportedDocuments.Services.Config;

namespace NeuronExportedDocuments.Interfaces
{
    public interface IConfig
    {
        MailSetting MailSetting { get; }
    }
}