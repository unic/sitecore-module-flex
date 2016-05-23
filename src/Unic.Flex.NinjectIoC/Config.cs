namespace Unic.Flex.NinjectIoC
{
    using Glass.Mapper.Sc;
    using Ninject.Modules;
    using Ninject.Web.Common;
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
            this.Bind<ILogger>().To<SitecoreLogger>().InSingletonScope();

            // business logic
            this.Bind<IContextService>().To<ContextService>();
            this.Bind<IPresentationService>().To<PresentationService>();
            this.Bind<IPlugsService>().To<PlugsService>();
            this.Bind<ITaskService>().To<TaskService>();
            this.Bind<IAnalyticsService>().To<AnalyticsService>();
            this.Bind<ICultureService>().To<CultureService>();

            // data access
            this.Bind<IDictionaryRepository>().To<DictionaryRepository>().InSingletonScope();
            this.Bind<IFormRepository>().To<FormRepository>();
            this.Bind<IUserDataRepository>().To<UserDataRepository>();
            this.Bind<IUnitOfWork>().To<UnitOfWork>();

            // model binding and converting
            this.Bind<IViewMapper>().To<ViewMapper>().InSingletonScope();

            this.Bind<FormModelBinder>().ToSelf();
            this.Bind<ListModelBinder>().ToSelf();
            this.Bind<FieldModelBinder>().ToSelf();
            this.Bind<UploadedFileModelBinder>().ToSelf();
            this.Bind<DateTimeModelBinder>().ToSelf();
            this.Bind<DecimalModelBinder>().ToSelf();

            // context
            this.Bind<IFlexContext>().To<FlexContext>().InRequestScope();

            // mailing
            this.Bind<IMailRepository>().To<MailRepository>();
            this.Bind<IMailService>().To<MailService>();

            // helpers
            this.Bind<IUrlService>().To<UrlService>().InSingletonScope();

            // implementation classes
            this.Bind<ISavePlugMailer>().To<SavePlugMailer>();
            this.Bind<ISaveToDatabaseService>().To<SaveToDatabaseService>();

            // third party classes
            this.Bind<IConfigurationManager>().To<ConfigurationManager>().InSingletonScope();
            this.Bind<ISitecoreContext>()
                .To<SitecoreContext>()
                .InRequestScope()
                .WithConstructorArgument("contextName", Constants.GlassMapperContextName);
        }
    }
}
