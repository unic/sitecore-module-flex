namespace Unic.Flex.Model.GlassExtensions.Attributes
{
    using System.Reflection;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Attribute to fallback a sitecore field to a dictionary entry if field value was empty.
    /// </summary>
    public class SitecoreDictionaryFallbackFieldAttribute : SitecoreFieldAttribute
    {
        /// <summary>
        /// The dictionary key
        /// </summary>
        private readonly string dictionaryKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreDictionaryFallbackFieldAttribute"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="dictionarykey">The dictionary key.</param>
        public SitecoreDictionaryFallbackFieldAttribute(string fieldName, string dictionarykey) : base(fieldName)
        {
            this.dictionaryKey = dictionarykey;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>Configuration of the current property</returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreDictionaryFallbackFieldConfiguration();
            this.Configure(propertyInfo, config);
            config.DictionaryKey = this.dictionaryKey;
            return config;
        }
    }
}
