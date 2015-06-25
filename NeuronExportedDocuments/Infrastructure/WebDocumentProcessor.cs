using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        private IDBUnitOfWork Database { get; set; }

        private IWebLogger Log { get; set; }
        public WebDocumentProcessor(IDBUnitOfWork database, IWebLogger logger)
        {
            Database = database;
            Log = logger;
        }
        public bool SendEmail(ServiceDocument doc)
        {

            try
            {
            // настройки smtp-сервера, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("pavel.shilan@aljur.com", "egkio329gul");

                // наш email с заголовком письма
                MailAddress from = new MailAddress("pavel.shilan@aljur.com", "Test");
                // кому отправляем
                MailAddress to = new MailAddress(doc.DeliveryEMail);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Test mail";
                // текст письма
                m.Body = string.Format("Document ID: {0}, document pass: {1}", doc.PublishId, doc.PublishPassword);
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
                if (SendEmail(doc))
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
                    if (!Database.ServiceDocuments.Find((document) => (document.PublishId == id.ToString())).Any())
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
                doc.PublishPassword = GetPass();
                doc.Status = ExportedDocStatus.Published;
            }
            return result;
        } 
    }
}