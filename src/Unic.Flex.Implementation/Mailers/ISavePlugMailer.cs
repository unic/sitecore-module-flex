namespace Unic.Flex.Implementation.Mailers
{
    using Mvc.Mailer;
    using Unic.Flex.Implementation.Plugs.SavePlugs;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Interface for the save plug mvc mailer
    /// </summary>
    public interface ISavePlugMailer
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="plug">The plug.</param>
        /// <returns>Message to be sent over the mvc mailer</returns>
        MvcMailMessage GetMessage(Form form, SendEmail plug);
    }
}
