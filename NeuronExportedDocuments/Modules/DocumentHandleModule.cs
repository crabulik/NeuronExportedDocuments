using System;
using System.Net.Mail;
using System.Timers;
using System.Web;
using NeuronExportedDocuments.DAL.Interfaces;
using NeuronExportedDocuments.Models;
using NeuronExportedDocuments.Models.Enums;

namespace NeuronExportedDocuments.Modules
{
    public class DocumentHandleModule: IHttpModule
    {
        static Timer timer;
        long interval = 30000; //30 секунд

        private IUnitOfWork Database { get; set; }

        public DocumentHandleModule(IUnitOfWork database)
        {
            Database = database;
        }

        public void Init(HttpApplication app)
        {
            timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = interval;
            timer.Elapsed += HandleDocuments;
            timer.Enabled = true;
        }

        private bool SendEmail(ServiceDocument doc)
        {
            
                DateTime dd = DateTime.Now;
                if (dd.Hour == 1 && dd.Minute == 30 )
                {
                    // настройки smtp-сервера, с которого будем отправлять письмо
                    SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.aljur.com", 587);
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("pavel.shilan@aljur.com", "egkio02gul");

                    // наш email с заголовком письма
                    MailAddress from = new MailAddress("pavel.shilan@aljur.com", "Test");
                    // кому отправляем
                    MailAddress to = new MailAddress("crabulik@gmail.ru");
                    // создаем объект сообщения
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Test mail";
                    // текст письма
                    m.Body = "Рассылка сайта";
                    smtp.Send(m);

                }
            return false;
        }

        private bool SendDocInfo(ServiceDocument doc)
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

        private bool PublishDocument(ServiceDocument doc)
        {
            
            return false;
        }

        private void HandleDocuments(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var changed = false;
            var unhandledDocs =
                Database.ServiceDocuments.Find((document) => document.Status == ExportedDocStatus.Unhandled);
            foreach (var unhandledDoc in unhandledDocs)
            {
                if (PublishDocument(unhandledDoc))
                {
                    Database.ServiceDocuments.Update(unhandledDoc);
                    changed = true;
                }
            }
            if (changed)
                Database.Save();

            changed = false;
            var publishedDocs =
                Database.ServiceDocuments.Find((document) => document.Status == ExportedDocStatus.Published);
            foreach (var publishedDoc in publishedDocs)
            {
                if (SendDocInfo(publishedDoc))
                {
                    Database.ServiceDocuments.Update(publishedDoc);
                    changed = true;
                }
            }
            if (changed)
                Database.Save();

            timer.Enabled = true;
        }

        public void Dispose()
        {
            timer.Elapsed -= HandleDocuments;
            timer.Close();
        }
    }
}