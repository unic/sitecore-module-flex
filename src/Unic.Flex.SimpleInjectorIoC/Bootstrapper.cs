namespace Unic.Flex.SimpleInjectorIoC
{
    using Glass.Mapper.Sc;
    using SimpleInjector;
    using SimpleInjector.Diagnostics;
    using Unic.Configuration;
    using Unic.Flex.Core.Analytics;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.Database;
    using Unic.Flex.Core.Definitions;
    using Unic.Flex.Core.Globalization;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Core.Mailing;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Core.ModelBinding;
    using Unic.Flex.Core.Plugs;
    using Unic.Flex.Core.Presentation;
    using Unic.Flex.Implementation.Database;
    using Unic.Flex.Implementation.Mailers;

    /// <summary>
    /// Bootstrapper for simple injector registrations
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Initializes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Initialize(Container container)
        {
            // logging
            container.Register<ILogger, SitecoreLogger>(Lifestyle.Singleton);
            
            // business logic
            container.Register<IContextService, ContextService>();
            container.Register<IPresentationService, PresentationService>();
            container.Register<IPlugsService, PlugsService>();
            container.Register<ITaskService, TaskService>();
            container.Register<IAnalyticsService, AnalyticsService>();
            container.Register<ICultureService, CultureService>();
            
            // data access
            container.Register<IDictionaryRepository, DictionaryRepository>(Lifestyle.Singleton);
            container.Register<IFormRepository, FormRepository>();
            container.Register<IUserDataRepository, UserDataRepository>();
            container.Register<IUnitOfWork, UnitOfWork>();

            // model binding and converting
            container.Register<IViewMapper, ViewMapper>(Lifestyle.Singleton);

            container.Register<FormModelBinder>();
            container.Register<ListModelBinder>();
            container.Register<FieldModelBinder>();
            container.Register<UploadedFileModelBinder>();
            container.Register<DateTimeModelBinder>();
            container.Register<DecimalModelBinder>();
            
            // context
            container.Register<IFlexContext, FlexContext>(Lifestyle.Scoped);

            // mailing
            container.Register<IMailRepository, MailRepository>();
            container.Register<IMailService, MailService>();

            // helpers
            container.Register<IUrlService, UrlService>(Lifestyle.Singleton);

            // implementation classes
            container.Register<ISavePlugMailer, SavePlugMailer>();
            container.Register<ISaveToDatabaseService, SaveToDatabaseService>();

            // third party classes
            container.Register<IConfigurationManager>(() => new ConfigurationManager(), Lifestyle.Singleton);
            container.Register<ISitecoreContext>(() => new SitecoreContext(Constants.GlassMapperContextName), Lifestyle.Scoped);

            // suppress warnings
            container.GetRegistration(typeof(IPlugsService)).Registration.SuppressDiagnosticWarning(DiagnosticType.LifestyleMismatch, "Bad application design, but we can't change this now without huge amount of testing...");
            container.GetRegistration(typeof(IContextService)).Registration.SuppressDiagnosticWarning(DiagnosticType.LifestyleMismatch, "Bad application design, but we can't change this now without huge amount of testing...");
            
            container.GetRegistration(typeof(IUnitOfWork)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Needed outside of a http context, so needs to be transient");
        }
    }
}
