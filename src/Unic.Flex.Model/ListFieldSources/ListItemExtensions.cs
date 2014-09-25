namespace Unic.Flex.Model.ListFieldSources
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for ListItem class
    /// </summary>
    public static class ListItemExtensions
    {
        /// <summary>
        /// Converts the domain model list items to Mvc select list items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>List of select list items.</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<ListItem> items)
        {
            foreach (var item in items)
            {
                yield return new SelectListItem { Selected = item.Selected, Text = item.Text, Value = item.Value };
            }
        }
    }
}
