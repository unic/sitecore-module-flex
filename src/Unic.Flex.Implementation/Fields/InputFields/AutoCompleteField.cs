namespace Unic.Flex.Implementation.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields.InputFields;

    /// <summary>
    /// Field for adding autocompletion.
    /// </summary>
    [SitecoreType(TemplateId = "{A9FD64D4-F178-498F-9BD1-D6264FD4B452}")]
    public class AutoCompleteField : InputField<string>
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
