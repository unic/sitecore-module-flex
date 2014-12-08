namespace Unic.Flex.Website.App_Start
{
    using System.Web.Hosting;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Configuration.Attributes;
    using Glass.Mapper.Pipelines.ObjectConstruction;
    using Glass.Mapper.Sc.CastleWindsor;
    using Sitecore.Pipelines;
    using Unic.Flex.Definitions;

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
            // create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            // install the custom services
            this.CastleConfig(resolver.Container);

            // create a context
            var context = Context.Create(resolver, Constants.GlassMapperContextName);
            context.Load(this.GlassLoaders());
        }

        /// <summary>
        /// Loads all glass configurations.
        /// </summary>
        /// <returns>Array with configuration loaders</returns>
        protected virtual IConfigurationLoader[] GlassLoaders()
        {
            var model = new AttributeConfigurationLoader(HostingEnvironment.MapPath("/bin/Unic.Flex.Model.dll"));
            var implementations = new AttributeConfigurationLoader(HostingEnvironment.MapPath("/bin/Unic.Flex.Implementation.dll"));
            return new IConfigurationLoader[] { model, implementations };
        }

        /// <summary>
        /// Install Glass Mapper into the dependency injection container.
        /// </summary>
        /// <param name="container">The container.</param>
        protected virtual void CastleConfig(IWindsorContainer container)
        {
            // get new config
            var config = new Config();

            // register custom type mapper
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<Model.GlassExtensions.Handlers.SitecoreDictionaryFallbackFieldTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<Model.GlassExtensions.Handlers.SitecoreSharedFieldTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<Model.GlassExtensions.Handlers.SitecoreReusableFieldTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<Model.GlassExtensions.Handlers.SitecoreReusableChildrenTypeMapper>().LifeStyle.Transient);

            // register ninject object creation
            container.Register(Component.For<IObjectConstructionTask>().ImplementedBy<Pipelines.ObjectConstruction.NinjectInjectorTask>().LifestylePerWebRequest());

            // install the config
            container.Install(new SitecoreInstaller(config));
        }
    }
}