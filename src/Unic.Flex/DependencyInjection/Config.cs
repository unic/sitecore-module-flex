namespace Unic.Flex.DependencyInjection
{
    using System;
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Unic.Configuration;
    using Unic.Flex.Analytics;
    using Unic.Flex.Context;
    using Unic.Flex.Database;
    using Unic.Flex.Definitions;
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
            // logging
            Bind<ILogger>().To<SitecoreLogger>().InSingletonScope();
            
            // business logic
            Bind<IContextService>().To<ContextService>();
            Bind<IPresentationService>().To<PresentationService>();
            Bind<IPlugsService>().To<PlugsService>();
            Bind<ITaskService>().To<TaskService>();
            Bind<IAnalyticsService>().To<AnalyticsService>();
            Bind<ICultureService>().To<CultureService>();
            
            // data access
            Bind<IDictionaryRepository>().To<DictionaryRepository>();
            Bind<IFormRepository>().To<FormRepository>();
            Bind<IUserDataRepository>().To<UserDataRepository>();
            Bind<IUnitOfWork>().To<UnitOfWork>();

            // model binding and converting
            Bind<IModelBinder>().To<FormModelBinder>().Named(typeof(FormModelBinder).FullName);
            Bind<IModelBinder>().To<DateTimeModelBinder>().Named(typeof(DateTimeModelBinder).FullName);
            Bind<IModelConverterService>().To<ModelConverterService>().InSingletonScope();

            // context
            Bind<IFlexContext>().To<FlexContext>().InRequestScope();

            // mailing
            Bind<IMailRepository>().To<MailRepository>();
            Bind<IMailService>().To<MailService>();

            // helpers
            Bind<IUrlService>().To<UrlService>().InSingletonScope();

            // third party classes
            Bind<IConfigurationManager>().To<ConfigurationManager>().When(request => request.Target != null
                    && request.Target.Member.DeclaringType != null
                    && request.Target.Member.DeclaringType.FullName.StartsWith(Constants.RootNamespace));

            Bind<ISitecoreContext>()
                .To<SitecoreContext>()
                .When(request => request.Target != null
                    && request.Target.Member.DeclaringType != null
                    && request.Target.Member.DeclaringType.FullName.StartsWith(Constants.RootNamespace))
                .WithConstructorArgument("contextName", Constants.GlassMapperContextName);
        }
    }
}
