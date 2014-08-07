namespace Unic.Flex.DependencyInjection
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Unic.Configuration;
    using Unic.Flex.Context;
    using Unic.Flex.Globalization;
    using Unic.Flex.Logging;
    using Unic.Flex.Mapping;
    using Unic.Flex.ModelBinding;
    using Unic.Flex.Plugs;
    using Unic.Flex.Presentation;

    /// <summary>
    /// Ninject configuration module.
    /// </summary>
    public class Config : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            // logging
            Bind<ILogger>().To<SitecoreLogger>();
            
            // business logic
            Bind<IContextService>().To<ContextService>();
            Bind<IPresentationService>().To<PresentationService>();
            Bind<IPlugsService>().To<PlugsService>();
            
            // data access
            Bind<IDictionaryRepository>().To<DictionaryRepository>();
            Bind<IFormRepository>().To<FormRepository>();
            Bind<IUserDataRepository>().To<UserDataRepository>();

            // model binding and converting
            Bind<IModelBinder>().To<FormModelBinder>();
            Bind<IModelConverterService>().To<ModelConverterService>().InSingletonScope();

            // context
            Bind<ISitecoreContext>().To<SitecoreContext>().InRequestScope();

            // configuration module
            Bind<IConfigurationManager>().To<ConfigurationManager>();
        }
    }
}
