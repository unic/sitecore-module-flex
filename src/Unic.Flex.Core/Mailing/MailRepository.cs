namespace Unic.Flex.Core.Mailing
{
    using System.Net;
    using System.Net.Mail;
    using Mvc.Mailer;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;

    /// <summary>
    /// Implementation of repository for sending emails.
    /// </summary>
    public class MailRepository : IMailRepository
    {
        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void SendMail(MvcMailMessage message)
        {
            Assert.ArgumentNotNull(message, "message");
            message.Send(new SmtpClientWrapper(GetSmtpClient()));
        }

        /// <summary>
        /// Gets the Sitecore SMTP client.
        /// </summary>
        /// <returns>SmtpClient based on the Sitecore settings.</returns>
        private static SmtpClient GetSmtpClient()
        {
            var credentials = new NetworkCredential(Settings.MailServerUserName, Settings.MailServerPassword);
            var smtpClient = new SmtpClient(Settings.MailServer, Settings.MailServerPort) { Credentials = credentials };
            return smtpClient;
        }
    }
}
