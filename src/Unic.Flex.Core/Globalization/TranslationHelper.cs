namespace Unic.Flex.Core.Globalization
{
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;

    /// <summary>
    /// Extensions for translation
    /// </summary>
    public static class TranslationHelper
    {
        /// <summary>
        /// Flexes the text.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Text translated by dictionary</returns>
        public static string FlexText(string key)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            return Translate.TextByDomain(Settings.GetSetting("Flex.DictionaryDomain"), key);
        }
    }
}
