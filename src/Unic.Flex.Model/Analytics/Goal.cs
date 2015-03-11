namespace Unic.Flex.Model.Analytics
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Data.Items;

    /// <summary>
    /// A analytics goal item
    /// </summary>
    [SitecoreType(TemplateId = "{475E9026-333F-432D-A4DC-52E03B75CB6B}")]
    public class Goal : ItemBase
    {
        /// <summary>
        /// Gets or sets the inner item.
        /// </summary>
        /// <value>
        /// The inner item.
        /// </value>
        [SitecoreItem]
        public virtual Item InnerItem { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [SitecoreField("Description")]
        public virtual string Description { get; set; }
    }
}
