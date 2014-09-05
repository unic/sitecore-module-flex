namespace Unic.Flex.Implementation.Configuration
{
    using Unic.Configuration;

    /// <summary>
    /// Send email plug configuration
    /// </summary>
    public class SendEmailPlugConfiguration : AbstractConfiguration
    {
        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        /// <value>
        /// The from address.
        /// </value>
        [Configuration(FieldName = "From")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        [Configuration(FieldName = "Reply To")]
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the to address.
        /// </summary>
        /// <value>
        /// The to address.
        /// </value>
        [Configuration(FieldName = "To")]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the CC address.
        /// </summary>
        /// <value>
        /// The CC address.
        /// </value>
        [Configuration(FieldName = "Cc")]
        public string Cc { get; set; }

        /// <summary>
        /// Gets or sets the BCC accress.
        /// </summary>
        /// <value>
        /// The BCC accress.
        /// </value>
        [Configuration(FieldName = "Bcc")]
        public string Bcc { get; set; }
    }
}
