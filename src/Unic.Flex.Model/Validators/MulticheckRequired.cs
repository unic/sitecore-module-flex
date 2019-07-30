namespace Unic.Flex.Model.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Required validator for fields with multiple options to check (checkboxlist)
    /// </summary>
    public class MulticheckRequired : ValidatorBase
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
            attributes.Add("data-val-multicheckrequired", this.ValidationMessage);
            return attributes;
        }
    }
}
