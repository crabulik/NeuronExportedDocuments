using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NeuronExportedDocuments.Interfaces;

namespace NeuronExportedDocuments.Infrastructure
{
    public class EmailService : IIdentityMessageService
    {
        private IConfig Config { get; set; }
        public EmailService(IConfig config)
        {
            Config = config;
        }
        public Task SendAsync(IdentityMessage message)
        {
            SmtpClient smtp = new SmtpClient(Config.MailSetting.SmtpServer, Config.MailSetting.SmtpPort);

            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = Config.MailSetting.EnableSsl;
            smtp.Credentials = new NetworkCredential(Config.MailSetting.SmtpUserName, Config.MailSetting.SmtpPassword);

            // наш email с заголовком письма
            MailAddress from = new MailAddress(Config.MailSetting.SmtpReply, Config.MailSetting.SmtpUser);
            // кому отправляем
            MailAddress to = new MailAddress(message.Destination);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);

            m.Subject = message.Subject;
            m.Body = message.Body;

            // Send:
            return smtp.SendMailAsync(m);
        }
    }
}