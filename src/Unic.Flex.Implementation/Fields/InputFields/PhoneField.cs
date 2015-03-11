namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.Fields.InputFields;

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

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/Phone";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singletextfield");

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("role", "textbox");
            this.Attributes.Add("type", "tel");
        }
    }
}
