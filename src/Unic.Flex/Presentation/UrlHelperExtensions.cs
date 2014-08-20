namespace Unic.Flex.Presentation
{
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for the Mvc url helper
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Gets the cache safed url content by adding assembly version to the content url
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="url">The URL.</param>
        /// <returns>Url with appended assembly version</returns>
        public static string CacheSafeContent(this UrlHelper urlHelper, string url)
        {
            if (urlHelper.RequestContext.HttpContext.Request.QueryString["minified"] == "false")
            {
                url = url.Replace(".min.", ".");
            }
            
            var contentUrl = urlHelper.Content(url);
            return string.Format("{0}?v={1}", contentUrl, Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", string.Empty));
        }
    }
}
