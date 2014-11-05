namespace Unic.Flex.Model.DataProviders
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a provider which provides items for a list field.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Gets the items for the list field.
        /// </summary>
        /// <returns>List of items</returns>
        IEnumerable<ListItem> GetItems();
    }
}
