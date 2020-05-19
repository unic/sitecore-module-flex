namespace Unic.Flex.Implementation.Configuration
{
    using System.Net.Mail;

    public class MailMessageConfiguration
    {
        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        /// <value>
        /// The from address.
        /// </value>
        public MailAddress From { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        public MailAddressCollection ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the to address.
        /// </summary>
        /// <value>
        /// The to address.
        /// </value>
        public MailAddressCollection To { get; set; }

        /// <summary>
        /// Gets or sets the CC address.
        /// </summary>
        /// <value>
        /// The CC address.
        /// </value>
        public MailAddressCollection Cc { get; set; }

        /// <summary>
        /// Gets or sets the BCC address.
        /// </summary>
        /// <value>
        /// The BCC address.
        /// </value>
        public MailAddressCollection Bcc { get; set; }
    }
}
