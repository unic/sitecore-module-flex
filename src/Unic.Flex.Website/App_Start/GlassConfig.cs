namespace Unic.Flex.Website.App_Start
{
    using System.Collections.Generic;
    using System.Web.Hosting;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Glass.Mapper;
    using Glass.Mapper.Configuration;
    using Glass.Mapper.Configuration.Attributes;
    using Glass.Mapper.Pipelines.ObjectConstruction;
    using Glass.Mapper.Sc.CastleWindsor;
    using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
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

            // register parameters
            container.Register(Component.For<IEnumerable<ISitecoreQueryParameter>>().ImplementedBy<List<ItemPathParameter>>().LifeStyle.Transient);
            container.Register(Component.For<IEnumerable<ISitecoreQueryParameter>>().ImplementedBy<List<ItemIdParameter>>().LifeStyle.Transient);
            container.Register(Component.For<IEnumerable<ISitecoreQueryParameter>>().ImplementedBy<List<ItemIdNoBracketsParameter>>().LifeStyle.Transient);
            container.Register(Component.For<IEnumerable<ISitecoreQueryParameter>>().ImplementedBy<List<ItemEscapedPathParameter>>().LifeStyle.Transient);
            container.Register(Component.For<IEnumerable<ISitecoreQueryParameter>>().ImplementedBy<List<ItemDateNowParameter>>().LifeStyle.Transient);

            // register custom type mapper
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<SitecoreDictionaryFallbackFieldTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<SitecoreSharedFieldTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<SitecoreSharedQueryTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<SitecoreReusableFieldTypeMapper>().LifeStyle.Transient);
            container.Register(Component.For<AbstractDataMapper>().ImplementedBy<SitecoreReusableChildrenTypeMapper>().LifeStyle.Transient);

            // register IoC object creation
            container.Register(Component.For<IObjectConstructionTask>().ImplementedBy<DependencyInjectorTask>().LifestylePerWebRequest());

            // install the config
            container.Install(new SitecoreInstaller(config));
        }
    }
}