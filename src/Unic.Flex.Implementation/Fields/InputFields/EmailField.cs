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
            this.DefaultValidators.Add(new EmailValidator());
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override string DefaultValue { get; set; }
    }
}
