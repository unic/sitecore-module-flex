namespace Unic.Flex.Implementation.Configuration
{
    using Unic.Configuration;

    public class SendEmailPlugConfiguration : AbstractConfiguration
    {
        [Configuration(FieldName = "From")]
        public string From { get; set; }

        [Configuration(FieldName = "To")]
        public string To { get; set; }

        [Configuration(FieldName = "Cc")]
        public string Cc { get; set; }

        [Configuration(FieldName = "Bcc")]
        public string Bcc { get; set; }
    }
}
