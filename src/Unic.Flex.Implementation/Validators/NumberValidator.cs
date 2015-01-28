namespace Unic.Flex.Implementation.Validators
{
    using System;
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Validator for validating numbers.
    /// </summary>
    [SitecoreType(TemplateId = "{29129AFA-3651-4A7F-BA87-CF1DEEDB48A5}")]
    public class NumberValidator : IValidator
    {
        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public virtual string DefaultValidationMessageDictionaryKey
        {
            get
            {
                return "Please enter a valid number";
            }
        }
        
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Please enter a valid number")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the start of the range to validate.
        /// </summary>
        /// <value>
        /// The start of the range.
        /// </value>
        [SitecoreField("Number Range Start")]
        public virtual decimal NumberRangeStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the range to validate.
        /// </summary>
        /// <value>
        /// The end of the range.
        /// </value>
        [SitecoreField("Number Range End")]
        public virtual decimal NumberRangeEnd { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public virtual bool IsValid(object value)
        {
            if (value == null) return true;
            if (string.IsNullOrWhiteSpace(value.ToString())) return true;

            try
            {
                var numberValue = Convert.ToDecimal(value);
                if (this.NumberRangeStart > 0 && numberValue < this.NumberRangeStart) return false;
                if (this.NumberRangeEnd > 0 && numberValue > this.NumberRangeEnd) return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public virtual IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();

            attributes.Add("data-val-number", this.ValidationMessage);

            if (this.NumberRangeStart > 0 || this.NumberRangeEnd > 0)
            {
                attributes.Add("data-val-range", this.ValidationMessage);
                if (this.NumberRangeStart > 0)
                {
                    attributes.Add("data-val-range-min", this.NumberRangeStart);
                    attributes.Add("min", this.NumberRangeStart);
                }

                if (this.NumberRangeEnd > 0)
                {
                    attributes.Add("data-val-range-max", this.NumberRangeEnd);
                    attributes.Add("max", this.NumberRangeEnd);
                }
            }

            return attributes;
        }
    }
}
