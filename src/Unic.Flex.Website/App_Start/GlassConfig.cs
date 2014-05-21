[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.GlassConfig), "Start")]

namespace Unic.Flex.Website
{
    using System.Web.Hosting;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Configuration.Attributes;
    using Glass.Mapper.Sc.CastleWindsor;
    using Unic.Flex.Model.GlassExtensions.Handlers;

    /// <summary>
    /// Configuration initializer for Sitecore Glass Mapper
    /// </summary>
    public static class GlassConfig
    {
        /// <summary>
        /// Called when application is stared -> configure Glass Mapper
        /// </summary>
        public static void Start()
        {
            // create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            // install the custom services
            CastleConfig(resolver.Container);

            // create a context
            var context = Context.Create(resolver);
            context.Load(GlassLoaders());
        }

        /// <summary>
        /// Loads all glass configurations.
        /// </summary>
        /// <returns>Array with configuration loaders</returns>
        private static IConfigurationLoader[] GlassLoaders()
        {
            var attributes = new AttributeConfigurationLoader(HostingEnvironment.MapPath("/bin/Unic.Flex.Model.dll"));
            return new IConfigurationLoader[] { attributes };
        }

        /// <summary>
        /// Install Glass Mapper into the dependency injection container.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void CastleConfig(IWindsorContainer container)
        {
            // get new config
            var config = new Config();

            // register custom type mapper
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<SitecoreDictionaryFallbackFieldTypeMapper>().LifeStyle.Transient);

            // install the config
            container.Install(new SitecoreInstaller(config));
        }
    }
}