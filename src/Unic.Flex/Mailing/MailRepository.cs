namespace Unic.Flex.Mailing
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using Mvc.Mailer;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Unic.Flex.Logging;

    /// <summary>
    /// Implementation of repository for sending emails.
    /// </summary>
    public class MailRepository : IMailRepository
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MailRepository(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// Boolean value weather the email could be handed over to the SMTP server.
        /// </returns>
        public virtual bool SendMail(MvcMailMessage message)
        {
            try
            {
                Assert.ArgumentNotNull(message, "message");
                message.Send(new SmtpClientWrapper(GetSmtpClient()));
            }
            catch (Exception exception)
            {
                this.logger.Error("Error while sending email", this, exception);
                return false;
            }

            return true;
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
