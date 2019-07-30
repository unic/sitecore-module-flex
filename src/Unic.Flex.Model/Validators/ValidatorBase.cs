using System.Collections.Generic;

namespace Unic.Flex.Model.Validators
{

    public abstract class ValidatorBase : IValidator
    {
        /// <summary>
        /// Gets the type of validation
        /// </summary>
        public virtual ValidationType Type => ValidationType.FieldValidation;

        public virtual string DefaultValidationMessageDictionaryKey => string.Empty;

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        public virtual string ValidationMessage { get; set; }


        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public virtual IDictionary<string, object> GetAttributes()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public abstract bool IsValid(object value);

    }
}
