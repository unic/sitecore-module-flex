namespace Unic.Flex.Model.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Unic.Flex.Model.Types;

    /// <summary>
    /// Validator to validate if field has a value
    /// </summary>
    public class RequiredValidator : IValidator
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
                return "Field is required";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public virtual bool IsValid(object value)
        {
            if (value == null) return false;

            var booleanValue = value as bool?;
            if (booleanValue != null) return (bool)value;

            var stringValue = value as string;
            if (stringValue != null) return !string.IsNullOrWhiteSpace(stringValue);

            var stringArrayValue = value as string[];
            if (stringArrayValue != null) return stringArrayValue.Any(v => !string.IsNullOrWhiteSpace(v));

            var fileValue = value as UploadedFile;
            if (fileValue != null) return true;

            return false;
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public IDictionary<string, object> GetAttributes()
        {
            //// todo: Client side validator must be sure to not validate file uploads if a file was uploaded before
            
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-required", this.ValidationMessage);
            attributes.Add("required", "required");
            attributes.Add("aria-required", true);
            return attributes;
        }
    }
}
