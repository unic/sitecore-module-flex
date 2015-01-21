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
        /// <param name="items">The items.</param>
        /// <returns>
        /// Sorted list of items.
        /// </returns>
        public IEnumerable<ListItem> Sort(IEnumerable<ListItem> items)
        {
            return items.OrderBy(item => item.Text);
        }
    }
}
