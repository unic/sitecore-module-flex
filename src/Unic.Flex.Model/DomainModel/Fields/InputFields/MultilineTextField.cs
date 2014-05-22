namespace Unic.Flex.Model.DomainModel.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// Multiline text field
    /// </summary>
    [SitecoreType(TemplateId = "{D5768466-2ADA-4C3C-B5C2-6B0299D08F2E}")]
    public class MultilineTextField : InputField
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
                return "Fields/InputFields/MultilineText";
            }
        }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        [SitecoreField("Rows")]
        public virtual int Rows { get; set; }
    }
}
