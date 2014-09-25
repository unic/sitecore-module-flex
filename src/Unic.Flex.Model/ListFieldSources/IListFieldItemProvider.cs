namespace Unic.Flex.Model.ListFieldSources
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a provider which provides items for a list field.
    /// </summary>
    public interface IListFieldItemProvider
    {
        /// <summary>
        /// Gets the items for the list field.
        /// </summary>
        /// <returns>List of items</returns>
        IEnumerable<ListItem> GetItems();
    }
}
