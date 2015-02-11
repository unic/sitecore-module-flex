namespace Unic.Flex.Core.Mapping.Ordering
{
    using System.Collections.Generic;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.SortOrder;

    /// <summary>
    /// Sort items by Sitecore sort.
    /// </summary>
    public class SitecoreSortOrder : ISortOrderStrategy
    {
        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <typeparam name="T">Type of the data item</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>
        /// Sorted list of items.
        /// </returns>
        public IEnumerable<T> Sort<T>(IEnumerable<T> items) where T : IDataItem
        {
            return items;
        }
    }
}
