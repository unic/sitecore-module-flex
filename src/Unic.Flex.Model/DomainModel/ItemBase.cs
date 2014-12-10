namespace Unic.Flex.Model.DomainModel
{
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System;

    /// <summary>
    /// Base class for all items.
    /// </summary>
    public class ItemBase
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [SitecoreId]
        public virtual Guid ItemId { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [SitecoreIgnore]
        public virtual string Id
        {
            get
            {
                return this.ItemId.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
    }
}
