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
        /// <typeparam name="T">Type of the data items.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>
        /// Sorted list of items.
        /// </returns>
        IEnumerable<T> Sort<T>(IEnumerable<T> items) where T : IDataItem;
    }
}
