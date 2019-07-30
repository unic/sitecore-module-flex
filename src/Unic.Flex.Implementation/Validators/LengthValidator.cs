namespace Unic.Flex.Implementation.Validators
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validators;

    /// <summary>
    /// Validates the input on specific length
    /// </summary>
    [SitecoreType(TemplateId = "{FAFB0D25-6930-4AEE-8BB1-3CE6E021C5EE}")]
    public class LengthValidator : ValidatorBase
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
                return "Length of input is not valid";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Length of input is not valid")]
        public override string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the minimum length.
        /// </summary>
        /// <value>
        /// The minimum length.
        /// </value>
        [SitecoreField("Minimum Length")]
        public virtual int MinimumLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>
        /// The maximum length.
        /// </value>
        [SitecoreField("Maximum Length")]
        public virtual int MaximumLength { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var stringValue = value as string;
            if (stringValue == null) return true;

            var length = stringValue.Length;
            if (length < this.MinimumLength) return false;
            if (this.MaximumLength > 0 && length > this.MaximumLength) return false;

            return true;
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

            attributes.Add("data-val-length", this.ValidationMessage);
            attributes.Add("data-val-length-min", this.MinimumLength);

            if (this.MaximumLength > 0)
            {
                attributes.Add("data-val-length-max", this.MaximumLength);
            }

            return attributes;
        }
    }
}
