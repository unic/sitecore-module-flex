namespace Unic.Flex.Implementation.Validators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Validation;

    /// <summary>
    /// Validates the uploaded files file type.
    /// </summary>
    [SitecoreType(TemplateId = "{14945398-D128-4E64-A0F8-7642252117C4}")]
    public class ValidFileTypeValidator : IValidator
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
                return "Invalid file type";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Invalid file type")]
        public virtual string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the valid file extensions.
        /// </summary>
        /// <value>
        /// The valid file extensions.
        /// </value>
        [SitecoreField("Valid File Extensions")]
        public virtual string ValidFileExtensions { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public virtual bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(this.ValidFileExtensions)) return true;

            var fileValue = value as HttpPostedFileBase;
            if (fileValue == null) return true;

            var extension = fileValue.FileName.Substring(fileValue.FileName.LastIndexOf('.'));
            return this.ValidFileExtensions.Split(',').Select(p => p.Trim()).Contains(extension);
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
            attributes.Add("accept", this.ValidFileExtensions);
            return attributes;
        }
    }
}
