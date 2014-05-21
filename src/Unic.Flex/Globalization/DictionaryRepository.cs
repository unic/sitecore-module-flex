namespace Unic.Flex.Globalization
{
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;

    public class DictionaryRepository : IDictionaryRepository
    {
        public string GetText(string key)
        {
            Assert.ArgumentNotNullOrEmpty(key, "key");
            return Translate.TextByDomain(Settings.GetSetting("Flex.DictionaryDomain"), key);
        }
    }
}
