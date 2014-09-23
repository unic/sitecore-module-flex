namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Number field
    /// </summary>
    [SitecoreType(TemplateId = "{C9C6AC45-7763-4851-A684-A075D2176FF7}")]
    public class NumberField : InputField<int?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberField"/> class.
        /// </summary>
        public NumberField()
        {
            this.DefaultValidators.Add(new NumberValidator());
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override int? DefaultValue { get; set; }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetValue(object value)
        {
            if (value == null)
            {
                base.SetValue(default(int?));
                return;
            }
            
            int intValue;
            base.SetValue(int.TryParse(value.ToString(), out intValue) ? intValue : default(int?));
        }
    }
}
