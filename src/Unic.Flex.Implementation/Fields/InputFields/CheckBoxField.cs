namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

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
            this.dictionaryRepository = Container.Resolve<IDictionaryRepository>();
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
        public override string TextValue
        {
            get
            {
                return this.Value ? this.dictionaryRepository.GetText("Yes") : this.dictionaryRepository.GetText("No");
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetValue(object value)
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
