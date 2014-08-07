namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Globalization;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// CheckBox field
    /// </summary>
    [SitecoreType(TemplateId = "{8583A797-3FE5-4247-B190-26E9BFD4D5A4}")]
    public class CheckBoxField : InputField<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxField"/> class.
        /// </summary>
        public CheckBoxField()
        {
            Container.Inject(this);
        }

        /// <summary>
        /// Gets or sets the dictionary repository.
        /// </summary>
        /// <value>
        /// The dictionary repository.
        /// </value>
        [Inject]
        public IDictionaryRepository DictionaryRepository { private get; set; }

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
                return this.Value ? this.DictionaryRepository.GetText("Yes") : this.DictionaryRepository.GetText("No");
            }
        }

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
                return "Fields/InputFields/CheckBox";
            }
        }
    }
}
