
namespace Unic.Flex.Model.GlassExtensions.Handlers
{
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.DataMappers;
    using Sitecore.Configuration;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// The type mapper for create fallback to a sitecore dictionary entry if value was empty.
    /// </summary>
    public class SitecoreDictionaryFallbackFieldTypeMapper : SitecoreFieldTypeMapper
    {
        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>Value found by the mapper</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var value = base.GetFieldValue(fieldValue, config, context);
            var stringValue = value as string;
            return string.IsNullOrWhiteSpace(stringValue) ? this.GetDictionaryText(config as SitecoreDictionaryFallbackFieldConfiguration) : value;
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreDictionaryFallbackFieldConfiguration && !configuration.PropertyInfo.PropertyType.IsGenericType;
        }

        /// <summary>
        /// Gets the dictionary text from Sitecore.
        /// </summary>
        /// <param name="config">The field configuration.</param>
        /// <returns>The dictionary key.</returns>
        private string GetDictionaryText(SitecoreDictionaryFallbackFieldConfiguration config)
        {
            Assert.ArgumentNotNull(config, "config");
            return Translate.TextByDomain(Settings.GetSetting("Flex.DictionaryDomain"), config.DictionaryKey);
        }
    }
}
