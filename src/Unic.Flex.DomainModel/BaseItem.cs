namespace Unic.Flex.DomainModel
{
    using System;
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// Base class for all items.
    /// </summary>
    [SitecoreType]
    public class BaseItem
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [SitecoreId]
        public virtual Guid ItemId { get; set; }
    }
}
