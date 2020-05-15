namespace Unic.Flex.Core.Mailing
{
    using MimeKit;
    using Mvc.Mailer;

    /// <summary>
    /// Mail repository to send an email
    /// </summary>
    public interface IMailRepository
    {
        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SendMail(MimeMessage message);
    }
}
