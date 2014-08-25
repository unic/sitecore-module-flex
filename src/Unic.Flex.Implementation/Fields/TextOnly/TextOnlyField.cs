namespace Unic.Flex.Implementation.Fields.TextOnly
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Fields;

    /// <summary>
    /// Field with text only content
    /// </summary>
    [SitecoreType(TemplateId = "{C2B3372D-F1BA-4A98-9FDE-7A2A0C0EE76B}")]
    public class TextOnlyField : FieldBase<string>
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [SitecoreField("Text")]
        public virtual string Text { get; set; }
    }
}
