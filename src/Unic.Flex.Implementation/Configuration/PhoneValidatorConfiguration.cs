namespace Unic.Flex.Implementation.Configuration
{
    using Unic.Configuration.Core;

    public class PhoneValidatorConfiguration : AbstractConfiguration
    {
        [Configuration(FieldName = "Regular Expression")]
        public virtual string RegularExpression { get; set; }
    }
}
