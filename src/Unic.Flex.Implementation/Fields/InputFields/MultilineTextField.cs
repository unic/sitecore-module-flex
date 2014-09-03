namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Multiline text field
    /// </summary>
    [SitecoreType(TemplateId = "{D5768466-2ADA-4C3C-B5C2-6B0299D08F2E}")]
    public class MultilineTextField : InputField<string>
    {
        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        [SitecoreField("Rows")]
        public virtual int Rows { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Default Value")]
        public override string DefaultValue { get; set; }
    }
}
