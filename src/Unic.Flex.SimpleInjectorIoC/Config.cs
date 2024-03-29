﻿namespace Unic.Flex.SimpleInjectorIoC
{
    using Core.MarketingAutomation;
    using Core.Utilities;
    using Glass.Mapper.Sc;
    using Glass.Mapper.Sc.Web;
    using Glass.Mapper.Sc.Web.Mvc;
    using Implementation.Services;
    using SimpleInjector;
    using SimpleInjector.Diagnostics;
    using Unic.Configuration.Core;
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
    public static class Config
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
            container.Register<IAsyncPlugExecutionService, AsyncPlugExecutionService>();
            container.Register<IServerOriginService, ServerOriginService>();

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
            container.Register<IMailerHelper, MailerHelper>();

            // helpers
            container.Register<IUrlService, UrlService>(Lifestyle.Singleton);
            container.Register<ITrackerWrapper, TrackerWrapper>();

            // implementation classes
            container.Register<ISavePlugMailer, SavePlugMailer>();
            container.Register<IDoubleOptinSavePlugMailer, DoubleOptinSavePlugMailer>();
            container.Register<ISaveToDatabaseService, SaveToDatabaseService>();
            container.Register<IDoubleOptinService, DoubleOptinService>();
            container.Register<IDoubleOptinLinkService, DoubleOptinLinkService>();

            // Marketing Automation
            container.Register<IMarketingAutomationContactService, MarketingAutomationContactService>();

            // third party classes
            container.Register<IConfigurationManager>(() => new ConfigurationManager(), Lifestyle.Singleton);
            container.Register<ISitecoreService>(GetService, Lifestyle.Scoped);
            container.Register<IRequestContext>(() => new RequestContext(GetService()), Lifestyle.Scoped);
            container.Register<IMvcContext>(() => new MvcContext(GetService()), Lifestyle.Scoped);

        }

        /// <summary>
        /// Get the Sitecore Service For Glassmapper
        /// In the Initial Pipeline Sitecore Database is null, so for kick-start the container we will set it manually on master or web, depends on the config.
        /// Afterwards it will uses the Context.Database
        /// </summary>
        /// <returns></returns>
        private static ISitecoreService GetService()
        {
            var databaseName = Sitecore.Context.Database?.Name;
            if (string.IsNullOrEmpty(databaseName))
            {
                databaseName = Sitecore.Configuration.Settings.GetSetting(Constants.FlexInitialDatabaseSetting, Constants.FlexInitialDatabaseName);
            }

            return new SitecoreService(databaseName, Constants.GlassMapperContextName);
        }

        /// <summary>
        /// Configure validation issues we are aware of and which should be suppressed.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Suppression(Container container)
        {
            container.GetRegistration(typeof(IPlugsService)).Registration.SuppressDiagnosticWarning(DiagnosticType.LifestyleMismatch, "Bad application design, but we can't change this now without huge amount of testing...");
            container.GetRegistration(typeof(IContextService)).Registration.SuppressDiagnosticWarning(DiagnosticType.LifestyleMismatch, "Bad application design, but we can't change this now without huge amount of testing...");

            container.GetRegistration(typeof(IUnitOfWork)).Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Needed outside of a http context, so needs to be transient");
        }
    }
}
