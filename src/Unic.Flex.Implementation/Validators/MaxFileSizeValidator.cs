namespace Unic.Flex.Implementation.Validators
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.Validators;

    /// <summary>
    /// Validator to validate the maximum file size of an uploaded file
    /// </summary>
    [SitecoreType(TemplateId = "{535AADCB-F275-4F37-A4D6-DA29FD83BDCA}")]
    public class MaxFileSizeValidator : ValidatorBase
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
                return "Max file size exceeded";
            }
        }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [SitecoreDictionaryFallbackField("Validation Message", "Max file size exceeded")]
        public override string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the file.
        /// </summary>
        /// <value>
        /// The maximum size of the file.
        /// </value>
        [SitecoreField("Max File Size")]
        public virtual double MaxFileSize { get; set; }

        /// <summary>
        /// Determines whether the specified value is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the value entered is valid, <c>false</c> otherwise
        /// </returns>
        public override bool IsValid(object value)
        {
            var fileValue = value as UploadedFile;
            if (fileValue == null) return true;

            var sizeInBytes = this.MaxFileSize * 1024 * 1024;
            return fileValue.ContentLength <= sizeInBytes;
        }
    }
}
