namespace Unic.Flex.Globalization
{
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;

    /// <summary>
    /// Service containign data access to Sitecore dictionaries.
    /// </summary>
    public class DictionaryRepository : IDictionaryRepository
    {
        /// <summary>
        /// Gets the text from a dictionary entry.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Dictionary entry value
        /// </returns>
        public string GetText(string key)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            return Translate.TextByDomain(Settings.GetSetting("Flex.DictionaryDomain"), key);
        }
    }
}
