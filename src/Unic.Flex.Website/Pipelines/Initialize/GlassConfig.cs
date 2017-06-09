namespace Unic.Flex.Website.Initialize
{
    using System.Web.Hosting;
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Configuration.Attributes;
    using Glass.Mapper.Maps;
    using Glass.Mapper.Sc.Configuration.Fluent;
    using Glass.Mapper.Sc.IoC;
    using Sitecore.Pipelines;
    using Unic.Flex.Core.Definitions;
    using Unic.Flex.Core.Pipelines.ObjectConstruction;
    using Unic.Flex.Model.GlassExtensions.Handlers;

    /// <summary>
    /// Configuration initializer for Sitecore Glass Mapper. This is called within the "initialize" pipeline of Sitecore.
    /// </summary>
    public class GlassConfig
    {
        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public virtual void Process(PipelineArgs args)
        {
            // create a resolver and register the context
            var resolver = this.CreateResolver();
            var context = Context.Create(resolver, Core.Definitions.Constants.GlassMapperContextName);

            // load configurations
            this.Configure(resolver);

            // get configuration loader
            var configurationLoader = this.GetConfigurationLoader(resolver);
            if (configurationLoader != null)
            {
                context.Load(configurationLoader);
            }

            // load the assemblies with Glass components
            context.Load(this.GetAssemblies());
        }

        /// <summary>
        /// Creates the resolver.
        /// </summary>
        /// <returns>Glass Dependency Resolver</returns>
        protected virtual IDependencyResolver CreateResolver()
        {
            var config = new Glass.Mapper.Sc.Config();

            var dependencyResolver = new DependencyResolver(config);

            return dependencyResolver;
        }

        /// <summary>
        /// Configures the Glass resolver.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        protected virtual void Configure(IDependencyResolver resolver)
        {
            // add tasks
            resolver.ObjectConstructionFactory.Insert(0, () => new DependencyInjectorTask());
            
            // add data mappers
            resolver.DataMapperFactory.Insert(0, () => new SitecoreSharedFieldTypeMapper());
            resolver.DataMapperFactory.Insert(0, () => new SitecoreDictionaryFallbackFieldTypeMapper());
            resolver.DataMapperFactory.Insert(0, () => new SitecoreSharedQueryTypeMapper(resolver.QueryParameterFactory.GetItems()));
            resolver.DataMapperFactory.Insert(0, () => new SitecoreReusableFieldTypeMapper());
            resolver.DataMapperFactory.Insert(0, () => new SitecoreReusableChildrenTypeMapper());
        }

        /// <summary>
        /// Get the configuration loader.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        protected virtual IConfigurationLoader GetConfigurationLoader(IDependencyResolver resolver)
        {
            var dependencyResolver = resolver as DependencyResolver;
            if (dependencyResolver == null)
            {
                return null;
            }

            var configurationMap = new ConfigurationMap(dependencyResolver);
            return configurationMap.GetConfigurationLoader<SitecoreFluentConfigurationLoader>();
        }

        /// <summary>
        /// Get all assemblies we need to load.
        /// </summary>
        /// <returns>Array with assemblies to load</returns>
        protected virtual IConfigurationLoader[] GetAssemblies()
        {
            var model = new AttributeConfigurationLoader(HostingEnvironment.MapPath("/bin/Unic.Flex.Model.dll"));
            var implementations = new AttributeConfigurationLoader(HostingEnvironment.MapPath("/bin/Unic.Flex.Implementation.dll"));
            return new IConfigurationLoader[] { model, implementations };
        }
    }
}