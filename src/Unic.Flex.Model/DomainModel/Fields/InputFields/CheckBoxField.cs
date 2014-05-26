namespace Unic.Flex.Model.DomainModel.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// CheckBox field
    /// </summary>
    [SitecoreType(TemplateId = "{8583A797-3FE5-4247-B190-26E9BFD4D5A4}")]
    public class CheckBoxField : InputField<bool>
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/CheckBox";
            }
        }
    }
}
