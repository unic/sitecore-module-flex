namespace Unic.Flex.Context
{
    using System.Linq;
    using Unic.Flex.Model;

    /// <summary>
    /// Extensions for url related objects.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Url of the current item appended to the current form context</returns>
        public static string GetUrl(this ItemBase item)
        {
            var context = FlexContext.Current;

            if (context.Item == null) return item.Url;
            if (context.Form == null) return item.Url;

            return string.Join("/", context.Item.Url, item.Url.Split('/').Last());
        }
    }
}
