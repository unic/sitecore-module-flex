﻿namespace Unic.Flex.Model
{
    using System;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// Base class for all items.
    /// </summary>
    [SitecoreType]
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
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
    }
}
