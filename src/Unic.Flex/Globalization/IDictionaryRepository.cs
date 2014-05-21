namespace Unic.Flex.Globalization
{
    /// <summary>
    /// Service containign data access to Sitecore dictionaries.
    /// </summary>
    public interface IDictionaryRepository
    {
        /// <summary>
        /// Gets the text from a dictionary entry.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Dictionary entry value</returns>
        string GetText(string key);
    }
}
