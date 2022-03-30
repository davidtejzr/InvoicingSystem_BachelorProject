using System.Net;
using System.Net.Mail;

namespace TEJ0017_FakturacniSystem
{
    public class EmailSender
    {
        private string EmailSenderr { get; set; }
        private string EmailReceiver { get}
        private string EmailSubject { get; set; }
        private string EmailBody { get; set; }
        private Stream EmailAttachment { get; set; }
        private int SmtpPort { get; set; }
        private string SmtpHost { get; set; }

        public EmailSender(string receiver, string subject, string body)
        {
            //email 
            this.EmailSenderr = "david.tejzr@seznam.cz";
            this.EmailReceiver = receiver;
            this.EmailSubject = subject;
            this.EmailBody = body;

            //smtp
            this.SmtpPort = 587;
            this.SmtpHost = "smtp.gmail.com";
        }

        public void SendEmail()
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            Attachment attachment = new Attachment(this.EmailAttachment, "Faktura");

            mailMessage.From = new MailAddress(this.EmailSenderr);
            mailMessage.To.Add(new MailAddress(this.EmailReceiver));
            mailMessage.Subject = this.EmailSubject;
            mailMessage.Body = this.EmailBody;

            smtpClient.Port = this.SmtpPort;
            smtpClient.Host = this.SmtpHost;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("email", "password");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(mailMessage);
        }

    }

}
