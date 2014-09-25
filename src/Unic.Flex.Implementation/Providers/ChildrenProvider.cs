namespace Unic.Flex.Implementation.Providers
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.ListFieldSources;

    /// <summary>
    /// Provider for list field items which lists all child items.
    /// </summary>
    [SitecoreType(TemplateId = "{66926539-DC85-4A9F-BD60-8A950DD842B7}")]
    public class ChildrenProvider : IListFieldItemProvider
    {
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        [SitecoreChildren]
        public virtual IEnumerable<ListItem> Children { get; set; }

        /// <summary>
        /// Gets the items for the list field.
        /// </summary>
        /// <returns>
        /// List of items
        /// </returns>
        public IEnumerable<ListItem> GetItems()
        {
            return this.Children;
        }
    }
}
