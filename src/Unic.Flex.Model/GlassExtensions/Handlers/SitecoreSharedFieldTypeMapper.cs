namespace Unic.Flex.Model.GlassExtensions.Handlers
{
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.DataMappers;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Custom glass handler to load a field value with version count disabler (for shared items only)
    /// </summary>
    public class SitecoreSharedFieldTypeMapper : SitecoreFieldTypeMapper
    {
        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>Field value object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            var scOptions = context.Options as GetOptionsSc;
            if (scOptions != null)
            {
                scOptions.VersionCount = false;
            }

            return base.GetFieldValue(fieldValue, config, context);
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
            return configuration is SitecoreSharedFieldConfiguration && !configuration.PropertyInfo.PropertyType.IsGenericType;
        }
    }
}
