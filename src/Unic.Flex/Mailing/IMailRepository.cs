namespace Unic.Flex.Mailing
{
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
        /// <returns>Boolean value weather the email could be handed over to the SMTP server.</returns>
        bool SendMail(MvcMailMessage message);
    }
}
