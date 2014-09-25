namespace Unic.Flex.Model.ListFieldSources
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// Represents an item in the list
    /// </summary>
    [SitecoreType(TemplateId = "{E4DE7509-97B6-4284-A9B9-BBC3452757F5}")]
    public class ListItem
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [SitecoreField("Text")]
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [SitecoreField("Value")]
        public virtual string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ListItem"/> is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Selected")]
        public virtual bool Selected { get; set; }
    }
}
