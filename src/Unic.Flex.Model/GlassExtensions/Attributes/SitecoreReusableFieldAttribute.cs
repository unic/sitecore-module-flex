namespace Unic.Flex.Model.GlassExtensions.Attributes
{
    using System.Reflection;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Attribute for a reusable field.
    /// </summary>
    public class SitecoreReusableFieldAttribute : SitecoreFieldAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreReusableFieldAttribute"/> class.
        /// </summary>
        public SitecoreReusableFieldAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreReusableFieldAttribute"/> class.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        public SitecoreReusableFieldAttribute(string fieldName) : base(fieldName)
        {
        }
        
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration or the attributes property info.</returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new SitecoreReusableFieldConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }
    }
}
