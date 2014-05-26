﻿namespace Unic.Flex.Model.DomainModel.Validators
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Validator to validate if field has a value
    /// </summary>
    public class RequiredValidator : IValidator
    {
        /// <summary>
        /// The validation message dictionary key
        /// </summary>
        public const string ValidationMessageDictionaryKey = "Field is required";
        
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

            var stringValue = value as string;
            return stringValue != null && !string.IsNullOrWhiteSpace(stringValue);
        }

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-required", this.ValidationMessage);
            return attributes;
        }
    }
}
