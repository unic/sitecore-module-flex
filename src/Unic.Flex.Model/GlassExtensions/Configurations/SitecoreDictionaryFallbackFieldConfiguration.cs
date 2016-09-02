namespace Unic.Flex.Model.GlassExtensions.Configurations
{
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration;

    /// <summary>
    /// Configuration for the SitecoreDictionaryFallbackField
    /// </summary>
    public class SitecoreDictionaryFallbackFieldConfiguration : SitecoreFieldConfiguration
    {
        /// <summary>
        /// Gets or sets the dictionary key.
        /// </summary>
        /// <value>
        /// The dictionary key.
        /// </value>
        public string DictionaryKey { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreDictionaryFallbackFieldConfiguration();
        }

        /// <summary>
        /// Makes a copy of the SitecoreFieldConfiguration
        /// </summary>
        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreDictionaryFallbackFieldConfiguration;
            config.DictionaryKey = this.DictionaryKey;
            base.Copy(copy);
        }
    }
}