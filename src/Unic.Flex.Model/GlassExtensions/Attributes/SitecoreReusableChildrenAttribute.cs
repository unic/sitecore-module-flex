namespace Unic.Flex.Model.GlassExtensions.Attributes
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Attribute for reusable children mapping
    /// </summary>
    public class SitecoreReusableChildrenAttribute : SitecoreChildrenAttribute
    {
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>
        /// The field AbstractPropertyConfiguration.
        /// </returns>
        public override Glass.Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreReusableChildrenConfiguration();
            base.Configure(propertyInfo, config);
            return config;
        }
    }
}
