using System.Net;
using System.Net.Mail;
using TEJ0017_FakturacniSystem.Models.Subject;

namespace TEJ0017_FakturacniSystem
{
    public class EmailSender
    {
        private string EmailSenderr { get; set; }
        private string EmailReceiver { get; set; }
        private string EmailSubject { get; set; }
        private string EmailBody { get; set; }
        private Stream EmailAttachment { get; set; }
        private int SmtpPort { get; set; }
        private string SmtpHost { get; set; }
        private string FileName { get; set; }

        public EmailSender(string receiver, string subject, string body, Stream attachment, string fileName)
        {
            //email konfigurace
            this.EmailSenderr = "david.tejzr@gmail.com";
            this.EmailReceiver = receiver;
            this.EmailSubject = subject;
            this.EmailBody = body;
            this.EmailAttachment = attachment;
            this.FileName = fileName;

            //smtp konfigurace
            this.SmtpPort = 587;
            this.SmtpHost = "smtp.gmail.com";
        }

        public bool SendEmail()
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                Attachment attachment = new Attachment(this.EmailAttachment, FileName);
                OurCompany ourCompany = OurCompany.getInstance();

                mailMessage.From = new MailAddress(this.EmailSenderr);
                mailMessage.To.Add(new MailAddress(this.EmailReceiver));
                mailMessage.Subject = this.EmailSubject;
                mailMessage.Body = this.EmailBody;
                mailMessage.Attachments.Add(attachment);

                smtpClient.Port = this.SmtpPort;
                smtpClient.Host = this.SmtpHost;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;

                //prihlasovaci udaje ulozeny v appData.json
                smtpClient.Credentials = new NetworkCredential(ourCompany.EmailSenderEmail, ourCompany.EmailSenderPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
