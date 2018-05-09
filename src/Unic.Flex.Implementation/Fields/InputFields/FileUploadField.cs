namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Model.Entities;
    using Unic.Flex.Model.Fields.InputFields;
    using Unic.Flex.Model.Types;

    /// <summary>
    /// File upload field
    /// </summary>
    [SitecoreType(TemplateId = "{6D95AE6A-4472-479C-B0AA-572A68C95E9A}")]
    public class FileUploadField : InputField<UploadedFile>
    {
        [SitecoreIgnore]
        public override string TextValue
        {
            get
            {
                if (this.Value == null) return base.TextValue;
                return !string.IsNullOrWhiteSpace(this.Value.FileName) ? this.Value.FileName : Model.Definitions.Constants.EmptyFlexFieldDefaultValue;
            }
        }

        [SitecoreIgnore]
        public override string ViewName => "Fields/InputFields/FileUpload";

        public override void SetValue(object value)
        {
            var file = value as File;
            if (file != null)
            {
                this.Value = new UploadedFile
                {
                    ContentLength = file.ContentLength,
                    ContentType = file.ContentType,
                    FileName = file.FileName,
                    Data = file.Data
                };
            }
            else
            {
                base.SetValue(value);
            }
        }

        public override void BindProperties()
        {
            base.BindProperties();

            this.AddCssClass("flex_fileinputfield");
        }
    }
}
