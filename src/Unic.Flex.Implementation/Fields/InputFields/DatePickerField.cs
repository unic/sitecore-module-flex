namespace Unic.Flex.Implementation.Fields.InputFields
{
    using System;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Implementation.Validators;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Field for a date picker
    /// </summary>
    [SitecoreType(TemplateId = "{5CBCDE45-F0A9-4F6D-8273-685249ABDDF6}")]
    public class DatePickerField : InputField<DateTime?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatePickerField"/> class.
        /// </summary>
        public DatePickerField()
        {
            this.DefaultValidators.Add(new DateValidator());
        }
        
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override DateTime? DefaultValue { get; set; }

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
                return !this.Value.HasValue ? base.TextValue : this.Value.Value.ToShortDateString();
            }
        }
    }
}
