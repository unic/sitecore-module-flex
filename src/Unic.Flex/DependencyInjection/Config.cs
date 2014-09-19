namespace Unic.Flex.DependencyInjection
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Unic.Configuration;
    using Unic.Flex.Context;
    using Unic.Flex.Database;
    using Unic.Flex.Globalization;
    using Unic.Flex.Logging;
    using Unic.Flex.Mailing;
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
            // todo: check if all scopes of these injetions are valid
            
            // logging
            Bind<ILogger>().To<SitecoreLogger>().InSingletonScope();
            
            // business logic
            Bind<IContextService>().To<ContextService>();
            Bind<IPresentationService>().To<PresentationService>();
            Bind<IPlugsService>().To<PlugsService>();
            Bind<ITaskService>().To<TaskService>().InRequestScope();
            
            // data access
            Bind<IDictionaryRepository>().To<DictionaryRepository>().InSingletonScope();
            Bind<IFormRepository>().To<FormRepository>();
            Bind<IUserDataRepository>().To<UserDataRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            // model binding and converting
            Bind<IModelBinder>().To<FormModelBinder>();
            Bind<IModelConverterService>().To<ModelConverterService>().InSingletonScope();

            // context
            Bind<ISitecoreContext>().To<SitecoreContext>().InRequestScope();
            Bind<IFlexContext>().To<FlexContext>().InRequestScope();

            // mailing
            Bind<IMailRepository>().To<MailRepository>();
            Bind<IMailService>().To<MailService>();

            // helpers
            Bind<IUrlService>().To<UrlService>().InSingletonScope();

            // configuration module
            Bind<IConfigurationManager>().To<ConfigurationManager>();
        }
    }
}
