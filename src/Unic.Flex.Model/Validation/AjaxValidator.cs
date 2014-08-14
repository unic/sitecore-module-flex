namespace Unic.Flex.Model.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// Abstract ajax validator to add needed properties and attribute for remote validation
    /// </summary>
    public abstract class AjaxValidator : IValidator
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
        public abstract string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the validator identifier.
        /// </summary>
        /// <value>
        /// The validator identifier.
        /// </value>
        [SitecoreId]
        public virtual Guid ValidatorId { get; set; }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public virtual FormMethod HttpMethod
        {
            get
            {
                return FormMethod.Post;
            }
        }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public abstract bool IsValid(object value);

        /// <summary>
        /// Gets the additional html attributes which should be rendered.
        /// </summary>
        /// <returns>
        /// Key-Value based dictionary with additional html attributes
        /// </returns>
        public IDictionary<string, object> GetAttributes()
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("data-val-remote", this.ValidationMessage);
            attributes.Add("data-val-remote-type", this.HttpMethod);
            attributes.Add("data-val-remote-additionalfields", "*.Value");

            // todo: this url has to be constructed automatically
            attributes.Add("data-val-remote-url", "/api/sitecore/Flex/AjaxValidator?validator=" + this.ValidatorId);
            return attributes;
        }
    }
}
