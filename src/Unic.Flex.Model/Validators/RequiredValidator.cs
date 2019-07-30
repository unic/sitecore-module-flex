namespace Unic.Flex.Model.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Validator to validate if field has a value
    /// </summary>
    public class RequiredValidator : ValidatorBase
    {
        /// <summary>
        /// Gets the default validation message dictionary key.
        /// </summary>
        /// <value>
        /// The default validation message dictionary key.
        /// </value>
        public override string DefaultValidationMessageDictionaryKey
        {
            get
            {
                return "Field is required";
            }
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var stringValue = value as string;
            if (stringValue != null) return !string.IsNullOrWhiteSpace(stringValue);

            var intValue = value as int?;
            if (intValue != null) return true;

            var decimalValue = value as decimal?;
            if (decimalValue != null) return true;

            var booleanValue = value as bool?;
            if (booleanValue != null) return (bool)value;

            var dateTimeValue = value as DateTime?;
            if (dateTimeValue != null) return true;

            var stringArrayValue = value as string[];
            if (stringArrayValue != null) return stringArrayValue.Any(v => !string.IsNullOrWhiteSpace(v));

            return false;
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public override IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-required", this.ValidationMessage);
            attributes.Add("required", "required");
            attributes.Add("aria-required", true);
            return attributes;
        }
    }
}
