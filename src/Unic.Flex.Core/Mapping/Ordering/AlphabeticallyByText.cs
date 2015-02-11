namespace Unic.Flex.Core.Mapping.Ordering
{
    using System.Collections.Generic;
    using System.Linq;
    using Unic.Flex.Model.DataProviders;
    using Unic.Flex.Model.SortOrder;

    /// <summary>
    /// Sort items alphabetically by text.
    /// </summary>
    public class AlphabeticallyByText : ISortOrderStrategy
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
            if (typeof(T) != typeof(ListItem)) return items;
            return items.Select(item => item as ListItem).OrderBy(item => item.Text) as IEnumerable<T>;
        }
    }
}
