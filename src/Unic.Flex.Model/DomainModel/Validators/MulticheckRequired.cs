﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.DomainModel.Validators
{
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Required validator for fields with multiple options to check (checkboxlist)
    /// </summary>
    public class MulticheckRequired : IValidator
    {
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
        public virtual IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-multicheckrequired", this.ValidationMessage);
            return attributes;
        }
    }
}
