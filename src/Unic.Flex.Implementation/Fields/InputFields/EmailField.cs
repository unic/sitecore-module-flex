namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Email field
    /// </summary>
    [SitecoreType(TemplateId = "{337AD4A8-DFEE-4CEB-851E-5133AB87269C}")]
    public class EmailField : InputField<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailField"/> class.
        /// </summary>
        public EmailField()
        {
            // todo: this validator shows two different validation messages on client side validation -> is 1 coming from jquery validation and one from the regex validator?
            this.DefaultValidators.Add(new EmailValidator { ValidationMessage = EmailValidator.ValidationMessageDictionaryKey });
        }
    }
}
