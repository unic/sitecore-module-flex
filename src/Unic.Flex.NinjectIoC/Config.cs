namespace Unic.Flex.NinjectIoC
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Glass.Mapper.Sc;
    using Ninject.Activation;
    using Ninject.Modules;
    using Ninject.Web.Common;
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
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;

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
            this.Bind<IFieldDependencyService>().To<FieldDependencyService>();
            this.Bind<IPresentationService>().To<PresentationService>();
            this.Bind<IPlugsService>().To<PlugsService>();
            this.Bind<ITaskService>().To<TaskService>();
            this.Bind<IAnalyticsService>().To<AnalyticsService>();
            this.Bind<ICultureService>().To<CultureService>();

            // data access
            this.Bind<IDictionaryRepository>().To<DictionaryRepository>();
            this.Bind<IFormRepository>().To<FormRepository>();
            this.Bind<IUserDataRepository>().To<UserDataRepository>();
            this.Bind<IUnitOfWork>().To<UnitOfWork>();

            // model binding and converting
            this.Bind<IModelBinder>().To<FormModelBinder>().Named(typeof(IFormViewModel).FullName);
            this.Bind<IModelBinder>().To<ListModelBinder>().Named(typeof(IList<ISectionViewModel>).FullName);
            this.Bind<IModelBinder>().To<ListModelBinder>().Named(typeof(IList<IFieldViewModel>).FullName);
            this.Bind<IModelBinder>().To<ListModelBinder>().Named(typeof(IList<SelectListItem>).FullName);
            this.Bind<IModelBinder>().To<FieldModelBinder>().Named(typeof(IFieldViewModel).FullName);
            this.Bind<IModelBinder>().To<UploadedFileModelBinder>().Named(typeof(UploadedFile).FullName);
            this.Bind<IModelBinder>().To<DateTimeModelBinder>().Named(typeof(DateTime?).FullName);
            this.Bind<IModelBinder>().To<DecimalModelBinder>().Named(typeof(decimal?).FullName);
            this.Bind<IModelConverterService>().To<ModelConverterService>().InSingletonScope();

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
            this.Bind<IConfigurationManager>().To<ConfigurationManager>();
            this.Bind<ISitecoreContext>().To<SitecoreContext>().WithConstructorArgument("contextName", Constants.GlassMapperContextName);
        }
    }
}
