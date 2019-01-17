namespace Unic.Flex.Implementation.Validators
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Validator for validating a valid phone number
    /// </summary>
    [SitecoreType(TemplateId = "{35946BDB-3616-4F81-B237-0E9ED7DCBB54}")]
    public class PhoneValidator : RegularExpressionValidator
    {
        private readonly string regularExpression;

        public PhoneValidator(string regularExpression)
        {
            this.regularExpression = regularExpression;
        }

        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public override string DefaultValidationMessageDictionaryKey
        {
            get
            {
                return "Phone number is invalid";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Phone number is invalid")]
        public override string ValidationMessage { get; set; }

        /// <summary>
        /// Gets the regular expression.
        /// </summary>
        /// <value>
        /// The regular expression.
        /// </value>
        public override string RegularExpression
        {
            get
            {
                return this.regularExpression;
            }
        }
    }
}
