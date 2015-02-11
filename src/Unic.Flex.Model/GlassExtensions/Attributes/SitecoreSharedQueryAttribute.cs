namespace Unic.Flex.Model.GlassExtensions.Attributes
{
    using System.Reflection;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Sitecore shared query attribute.
    /// </summary>
    public class SitecoreSharedQueryAttribute : SitecoreQueryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreSharedQueryAttribute"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public SitecoreSharedQueryAttribute(string query)
            : base(query)
        {
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// The configuration.
        /// </returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreSharedQueryConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }
    }
}
