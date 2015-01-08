namespace Unic.Flex.Core.Context
{
    /// <summary>
    /// Service class for url related code.
    /// </summary>
    public interface IUrlService
    {
        /// <summary>
        /// Adds the query string to current URL.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The concatinated URL</returns>
        string AddQueryStringToCurrentUrl(string key, string value);

        /// <summary>
        /// Gets the current URL.
        /// </summary>
        /// <returns>Current URL</returns>
        string GetCurrentUrl();
    }
}
