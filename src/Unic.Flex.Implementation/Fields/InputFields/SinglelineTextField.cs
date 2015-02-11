namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Singleline text field
    /// </summary>
    [SitecoreType(TemplateId = "{C706D294-ADC2-477F-B5E1-63EB1EF345E5}")]
    public class SinglelineTextField : InputField<string>
    {
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
