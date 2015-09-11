namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.Fields.InputFields;

    /// <summary>
    /// CheckBox field
    /// </summary>
    [SitecoreType(TemplateId = "{8583A797-3FE5-4247-B190-26E9BFD4D5A4}")]
    public class CheckBoxField : InputField<bool>
    {
        /// <summary>
        /// The dictionary repository
        /// </summary>
        private readonly IDictionaryRepository dictionaryRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxField"/> class.
        /// </summary>
        public CheckBoxField()
        {
            this.dictionaryRepository = DependencyResolver.Resolve<IDictionaryRepository>();
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override bool DefaultValue { get; set; }

        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        [SitecoreIgnore]
        public override string TextValue
        {
            get
            {
                return this.Value ? this.dictionaryRepository.GetText("Yes") : this.dictionaryRepository.GetText("No");
            }
        }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/CheckBox";
            }
        }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_singlecheckbox");

            this.Attributes.Add("aria-multiline", false);
            this.Attributes.Add("aria-checked", this.Value);
            this.Attributes.Add("role", "checkbox");

            /*
            this is an ugly hack, because of a b-u-g with jQuery validate and checkboxes which should not be required...
            because for a checkbox there is always an additional hidden field with value "false" and according to this,
            the value is always required (needs to be true or false). But this breaks the frontend...So we say that
            we don't need a validation with "data-val=false" if it is not required.
            */
            if (!this.IsRequired)
            {
                this.Attributes.Add("data-val", false);
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetValue(object value)
        {
            if (value is string)
            {
                bool boolValue;
                base.SetValue(bool.TryParse(value.ToString(), out boolValue) && boolValue);
                return;
            }

            base.SetValue(value);
        }
    }
}
