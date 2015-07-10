using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;
using NeuronExportedDocuments.Models.Interfaces;
using NeuronExportedDocuments.Resources;

namespace NeuronExportedDocuments.Infrastructure
{
    

    public class WebDocumentProcessor : IWebDocumentProcessor
    {
        static object _synclock = new object();
        static object _changeSynclock = new object();
        private IDBUnitOfWork Database { get; set; }
        private IConfig Config { get; set; }

        private IWebLogger Log { get; set; }
        private IServiceMessages ServicesMessages { get; set; }
        public WebDocumentProcessor(IDBUnitOfWork database, IWebLogger logger,
            IConfig config, IServiceMessages servicesMessages)
        {
            Database = database;
            Log = logger;
            Config = config;
            ServicesMessages = servicesMessages;
        }
        public bool SendEmail(ServiceDocument doc, string subject, string body)
        {

            try
            {
            // настройки smtp-сервера, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient(Config.MailSetting.SmtpServer, Config.MailSetting.SmtpPort);
                smtp.EnableSsl = Config.MailSetting.EnableSsl;
                smtp.Credentials = new NetworkCredential(Config.MailSetting.SmtpUserName, Config.MailSetting.SmtpPassword);

                // наш email с заголовком письма
                MailAddress from = new MailAddress(Config.MailSetting.SmtpReply, Config.MailSetting.SmtpUser);
                // кому отправляем
                MailAddress to = new MailAddress(doc.DeliveryEMail);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = subject;
                // текст письма
                m.Body = body;
                smtp.Send(m);
            }
            catch (Exception e)
            {
                Log.Error(MainExceptionMessages.rs_SendEmailError, e, doc);

                return false;
            }
            return true;
        }

        public bool SendDocInfo(ServiceDocument doc)
        {
            var result = false;
            if (doc.DeliveryEMail != string.Empty)
            {
                if (SendEmail(doc,
                    ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendCredentialsEmailSubject, doc),
                    ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendCredentialsEmailMessage, doc)))
                {
                    result = true;
                }
            }
            if (result)
            {
                doc.Status = ExportedDocStatus.InfoSented;
            }
            return result;
        }

        public bool TrySetPublishId(ServiceDocument doc)
        {
            var result = false;
            for (int i = 0; i < 100; i++)
            {
                var sourceString = DateTime.Now.ToLongTimeString() + doc.ServiceDocumentId +
                                   DateTime.Now.ToShortDateString() + doc.NeuronDbDocumentId;
                var id = sourceString.GetHashCode();
                if (id < 0)
                {
                    id = 0 - id;
                }
                if (id != 0)
                lock (_synclock)
                {
                    if (!Database.ServiceDocuments.GetQueryable().Any(document => (document.PublishId == id.ToString())))
                    {
                        result = true;
                    }
                }
                if (result)
                {
                    doc.PublishId = id.ToString();
                    return true;
                }
            }

            Log.Error(MainExceptionMessages.rs_GeneratingIdAttemptsOutOfBounds, doc);
            return false;
        }

        public string GetPass(int x = 8)
        {
            string pass = "";
            var r = new Random();
            while (pass.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }
            return pass;
        }

        public bool PublishDocument(ServiceDocument doc)
        {
            var result = false;
            result = TrySetPublishId(doc);
            if (result)
            {
                doc.PublishPassword = GetPass(Config.GeneralSettings.DocumentPassLength);
                doc.Status = ExportedDocStatus.Published;
            }
            return result;
        }

        public bool RepublishDocument(ServiceDocument doc)
        {
            var result = true;
            if (string.IsNullOrEmpty(doc.PublishId))
              result = TrySetPublishId(doc);
            if (result)
            {
                result = false;
                doc.PublishPassword = GetPass(Config.GeneralSettings.DocumentPassLength);

                if (doc.DeliveryEMail != string.Empty)
                {
                    if (SendEmail(doc,
                        ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendChangedCredentialsEmailSubject, doc),
                        ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendChangedCredentialsEmailMessage, doc)))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool ArchiveDocument(ServiceDocument doc, IDBUnitOfWork database = null)
        {

            var workDb = database ?? Database;

            foreach (var img in workDb.DocumentImages.GetQueryable().Where(p => p.ServiceDocumentId == doc.ServiceDocumentId))
            {
                workDb.DocumentImages.Delete(img.DocumentImageId);
            }
            if (database == null)
                workDb.Save();
            doc.PdfFileData = null;
            doc.ImageData = null;
            doc.ImagesInterpretation = null;
            doc.Status = ExportedDocStatus.InArchive;

            if (doc.DeliveryEMail != string.Empty)
            {
                SendEmail(doc,
                    ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendDocumentAccessExpiredSubject, doc),
                    ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendDocumentAccessExpiredMessage, doc));
            }
         
            return true;
        }

        public void BlockDocument(ServiceDocument doc, string userIpAddress)
        {
            var editDoc = Database.ServiceDocuments.Get(doc.ServiceDocumentId);

            if (!editDoc.IsBlocked)
            {
                editDoc.IsBlocked = true;
                editDoc.DocumentOperations.Add(new DocumentLogOperation
                {
                    OperationType = DocumentLogOperationType.Blocked,
                    ConnectionIpAddress = userIpAddress
                });
                Database.ServiceDocuments.Update(editDoc);
                Database.Save();

                if (doc.DeliveryEMail != string.Empty)
                {
                    SendEmail(doc,
                        ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendDocumentAccessBlockedSubject, doc),
                        ServicesMessages.FormatMessageByServiceDocument(ServiceMessageKey.SendDocumentAccessBlockedMessage, doc));
                }
            }
        }
    }
}