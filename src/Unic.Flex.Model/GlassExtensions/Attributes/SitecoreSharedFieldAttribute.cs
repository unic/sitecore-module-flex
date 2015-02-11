namespace Unic.Flex.Model.GlassExtensions.Attributes
{
    using System.Reflection;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Custom Sitecore field attribute to specify that the specific property should ignore the version check
    /// </summary>
    public class SitecoreSharedFieldAttribute : SitecoreFieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreSharedFieldAttribute"/> class.
        /// </summary>
        public SitecoreSharedFieldAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreSharedFieldAttribute"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public SitecoreSharedFieldAttribute(string fieldName) : base(fieldName)
        {
        }
        
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration or the attributes property info.</returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreSharedFieldConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }
    }
}
