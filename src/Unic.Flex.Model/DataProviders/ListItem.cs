namespace Unic.Flex.Model.DataProviders
{
    using Components;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// Represents an item in the list
    /// </summary>
    [SitecoreType(TemplateId = "{E4DE7509-97B6-4284-A9B9-BBC3452757F5}")]
    public class ListItem : IDataItem, ITooltip
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

        /// <summary>
        /// Gets or sets the cascading data provider.
        /// </summary>
        /// <value>
        /// The cascading data provider.
        /// </value>
        [SitecoreSharedField("Cascading Data Provider", Setting = SitecoreFieldSettings.InferType)]
        public virtual IDataProvider<ListItem> CascadingDataProvider { get; set; }

        /// <summary>
        /// Gets or sets the tooltip title.
        /// </summary>
        /// <value>
        /// The tooltip title.
        /// </value>
        [SitecoreDictionaryFallbackField("Tooltip Title", "Tooltip Title")]
        public virtual string TooltipTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text.
        /// </summary>
        /// <value>
        /// The tooltip text.
        /// </value>
        [SitecoreField("Tooltip Text")]
        public virtual string TooltipText { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        [SitecoreIgnore]
        public virtual Tooltip Tooltip { get; set; }
    }
}
