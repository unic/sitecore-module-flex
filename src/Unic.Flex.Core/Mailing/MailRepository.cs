namespace Unic.Flex.Core.Mailing
{
    using MailKit.Net.Smtp;
    using MimeKit;
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
        public virtual void SendMail(MimeMessage message)
        {
            Assert.ArgumentNotNull(message, "message");

            using (var client = GetSmtpClient())
            {
                client.Send(message);
                client.Disconnect(true);
            }
        }

        /// <summary>
        /// Gets the Sitecore SMTP client.
        /// </summary>
        /// <returns>SmtpClient based on the Sitecore settings.</returns>
        private static SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            client.Connect(Settings.MailServer, Settings.MailServerPort);

            var username = Settings.MailServerUserName;
            var password = Settings.MailServerPassword;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                client.Authenticate(username, password);
            }

            return client;
        }
    }
}
