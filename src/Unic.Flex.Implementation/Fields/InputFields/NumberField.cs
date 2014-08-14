﻿namespace Unic.Flex.Implementation.Fields.InputFields
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
            this.DefaultValidators.Add(new NumberValidator { ValidationMessage = NumberValidator.ValidationMessageDictionaryKey });
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        [SitecoreField("Min Value")]
        public virtual int MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        [SitecoreField("Max Value")]
        public virtual int MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        [SitecoreField("Step")]
        public virtual int Step { get; set; }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetValue(object value)
        {
            if (value == null)
            {
                this.Value = default(int?);
                return;
            }
            
            int intValue;
            this.Value = int.TryParse(value.ToString(), out intValue) ? intValue : (int?)null;
        }
    }
}
