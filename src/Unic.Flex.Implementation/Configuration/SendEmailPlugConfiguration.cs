namespace Unic.Flex.Implementation.Configuration
{
    using Unic.Configuration;

    public class SendEmailPlugConfiguration : AbstractConfiguration
    {
        [Configuration(FieldName = "From")]
        public string From { get; set; }
    }
}
