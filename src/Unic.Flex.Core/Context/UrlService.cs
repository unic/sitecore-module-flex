namespace Unic.Flex.Core.Context
{
    /// <summary>
    /// Service for url related functionalities.
    /// </summary>
    public class UrlService : IUrlService
    {
        /// <summary>
        /// Adds the query string to current URL.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The concatinated URL
        /// </returns>
        public virtual string AddQueryStringToCurrentUrl(string key, string value)
        {
            var rawUrl = this.GetCurrentUrl();
            return string.Format("{0}{1}{2}={3}", rawUrl, rawUrl.Contains("?") ? "&" : "?", key, value);
        }

        /// <summary>
        /// Gets the current URL.
        /// </summary>
        /// <returns>
        /// Current URL
        /// </returns>
        public virtual string GetCurrentUrl()
        {
            return Sitecore.Web.WebUtil.GetRawUrl();
        }
    }
}
