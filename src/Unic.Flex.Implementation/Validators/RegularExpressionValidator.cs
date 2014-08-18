﻿namespace Unic.Flex.Implementation.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Logging;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Regular expression validator
    /// </summary>
    [SitecoreType(TemplateId = "{0E0AAF18-7D98-4A59-AF1F-45A0652DF4C9}")]
    public class RegularExpressionValidator : IValidator
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
                return "Input is invalid";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Input is invalid")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the regular expression.
        /// </summary>
        /// <value>
        /// The regular expression.
        /// </value>
        [SitecoreField("Regular Expression")]
        public virtual string RegularExpression { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public bool IsValid(object value)
        {
            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            // todo: add regular expression validator for Sitecore item to validate correctly regex in the field
            
            try
            {
                var regex = new Regex(this.RegularExpression);
                var match = regex.Match(stringValue);
                return match.Success && match.Index == 0 && match.Length == stringValue.Length;
            }
            catch (ArgumentException exception)
            {
                var logger = Container.Resolve<ILogger>();
                logger.Error(string.Format("Regular expression '{0}' is not valid", this.RegularExpression), this, exception);
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
            attributes.Add("data-val-regex", this.ValidationMessage);
            attributes.Add("data-val-regex-pattern", this.RegularExpression);
            return attributes;
        }
    }
}
