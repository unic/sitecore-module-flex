namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;
    using Unic.Flex.Model.Types;

    /// <summary>
    /// File upload field
    /// </summary>
    [SitecoreType(TemplateId = "{6D95AE6A-4472-479C-B0AA-572A68C95E9A}")]
    public class FileUploadField : InputField<UploadedFile>
    {
        /// <summary>
        /// Gets the text value.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        public override string TextValue
        {
            get
            {
                if (this.Value == null) return base.TextValue;
                return !string.IsNullOrWhiteSpace(this.Value.FileName) ? this.Value.FileName : "-";
            }
        }
    }
}
