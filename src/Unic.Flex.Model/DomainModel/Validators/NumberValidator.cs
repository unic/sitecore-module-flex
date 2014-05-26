namespace Unic.Flex.Model.DomainModel.Validators
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
        /// The validation message dictionary key
        /// </summary>
        public const string ValidationMessageDictionaryKey = "Please enter a valid number";
        
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", ValidationMessageDictionaryKey)]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the start of the range to validate.
        /// </summary>
        /// <value>
        /// The start of the range.
        /// </value>
        [SitecoreField("Number Range Start")]
        public virtual int NumberRangeStart { get; set; }

        /// <summary>
        /// Gets or sets the end of the range to validate.
        /// </summary>
        /// <value>
        /// The end of the range.
        /// </value>
        [SitecoreField("Number Range End")]
        public virtual int NumberRangeEnd { get; set; }

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

            var stringValue = value as string;
            if (stringValue == null || string.IsNullOrWhiteSpace(stringValue)) return true;

            try
            {
                var numberValue = Convert.ToInt32(value);
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
        public IDictionary<string, object> GetAttributes()
        {
            // todo: handle range attributes
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-number", this.ValidationMessage);
            return attributes;
        }
    }
}
