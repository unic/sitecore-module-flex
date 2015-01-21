namespace Unic.Flex.Model.SortOrder
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DataProviders;

    /// <summary>
    /// A sort order strategy
    /// </summary>
    public interface ISortOrderStrategy
    {
        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>Sorted list of items.</returns>
        IEnumerable<ListItem> Sort(IEnumerable<ListItem> items);
    }
}
