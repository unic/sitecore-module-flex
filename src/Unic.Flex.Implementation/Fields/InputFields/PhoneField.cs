namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Field for phone number
    /// </summary>
    [SitecoreType(TemplateId = "{07AC3EF4-7E76-4454-B3BE-49FF647D9439}")]
    public class PhoneField : InputField<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneField"/> class.
        /// </summary>
        public PhoneField()
        {
            this.DefaultValidators.Add(new PhoneValidator());
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
