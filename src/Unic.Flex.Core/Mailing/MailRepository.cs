namespace Unic.Flex.Core.Mailing
{
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;
    using Mvc.Mailer;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using System;
    using System.Net;
    using NetMail = System.Net.Mail;

    /// <summary>
    /// Implementation of repository for sending emails.
    /// </summary>
    public class MailRepository : IMailRepository
    {
        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void SendMail(MimeMessage message)
        {
            Assert.ArgumentNotNull(message, "message");

            try
            {
                using (var client = GetMailKitSmtpClient())
                {
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>S
        /// Sends the mail message via System.Net.Mail. Method remains for backwards compatibility purposes,
        /// will be called by not async saveplugs 
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void SendMail(MvcMailMessage message)
        {
            try
            {
                Assert.ArgumentNotNull(message, "message");
                message.Send(new SmtpClientWrapper(GetSmtpClient()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Gets the MailKit SMTP client.
        /// </summary>
        /// <returns>SmtpClient based on the Sitecore settings.</returns>
        private static SmtpClient GetMailKitSmtpClient()
        {
            var client = new SmtpClient();
            client.Connect(Settings.MailServer, Settings.MailServerPort, SecureSocketOptions.None);

            var username = Settings.MailServerUserName;
            var password = Settings.MailServerPassword;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                client.Authenticate(username, password);
            }

            return client;
        }

        /// <summary>
        /// Gets the System.Net.Mail SMTP client.
        /// </summary>
        /// <returns>SmtpClient based on the Sitecore settings.</returns>
        private static NetMail.SmtpClient GetSmtpClient()
        {
            var credentials = new NetworkCredential(Settings.MailServerUserName, Settings.MailServerPassword);
            var smtpClient = new NetMail.SmtpClient(Settings.MailServer, Settings.MailServerPort) { Credentials = credentials };
            return smtpClient;
        }
    }
}
