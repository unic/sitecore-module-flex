namespace Unic.Flex.Model.GlassExtensions.Handlers
{
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.DataMappers;
    using Unic.Flex.Model.DomainModel.Fields;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Type mapper for reusable fields.
    /// </summary>
    public class SitecoreReusableFieldTypeMapper : SitecoreFieldTypeMapper
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
            var field = base.GetFieldValue(fieldValue, config, context);
            var reusableField = field as IField;
            if (reusableField == null) return field;
            return reusableField.ReusableComponent ?? reusableField;
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
            return configuration is SitecoreReusableFieldConfiguration && !configuration.PropertyInfo.PropertyType.IsGenericType;
        }
    }
}
