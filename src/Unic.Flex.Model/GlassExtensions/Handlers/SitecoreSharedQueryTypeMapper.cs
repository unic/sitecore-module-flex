namespace Unic.Flex.Model.GlassExtensions.Handlers
{
    using System.Collections.Generic;
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Configuration;
    using Glass.Mapper.Sc.DataMappers;
    using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
    using Unic.Flex.Model.GlassExtensions.Configurations;

    /// <summary>
    /// Maps a query property with disabled version count
    /// </summary>
    public class SitecoreSharedQueryTypeMapper : SitecoreQueryMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreSharedQueryTypeMapper"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public SitecoreSharedQueryTypeMapper(IEnumerable<ISitecoreQueryParameter> parameters)
            : base(parameters)
        {
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>
        /// The mapped object.
        /// </returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            mappingContext.Options.Lazy = LazyLoading.Disabled;
            var scOptions = mappingContext.Options as GetOptionsSc;
            if (scOptions != null)
            {
                scOptions.VersionCount = false;
            }

            return base.MapToProperty(mappingContext);
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreSharedQueryConfiguration;
        }
    }
}
