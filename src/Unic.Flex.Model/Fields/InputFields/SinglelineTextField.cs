namespace Unic.Flex.Model.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// Singleline text field
    /// </summary>
    [SitecoreType(TemplateId = "{C706D294-ADC2-477F-B5E1-63EB1EF345E5}")]
    public class SinglelineTextField : InputField
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
                return "Fields/InputFields/SinglelineText";
            }
        }
    }
}
