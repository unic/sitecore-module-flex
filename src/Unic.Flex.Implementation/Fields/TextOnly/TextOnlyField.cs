namespace Unic.Flex.Implementation.Fields.TextOnly
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Fields;

    /// <summary>
    /// Field with text only content
    /// </summary>
    [SitecoreType(TemplateId = "{C2B3372D-F1BA-4A98-9FDE-7A2A0C0EE76B}")]
    public class TextOnlyField : FieldBase<string>, IFieldWithoutPost
    {
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [SitecoreField("Text")]
        public override string DefaultValue { get; set; }

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
                return "Fields/TextOnlyFields/TextOnly";
            }
        }
    }
}
