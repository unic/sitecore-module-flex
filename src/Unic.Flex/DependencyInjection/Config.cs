namespace Unic.Flex.DependencyInjection
{
    using System;
    using System.Collections.Generic;
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
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
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
            Bind<IFieldDependencyService>().To<FieldDependencyService>();
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
            Bind<IModelBinder>().To<FormModelBinder>().Named(typeof(IFormViewModel).FullName);
            Bind<IModelBinder>().To<ListModelBinder>().Named(typeof(IList<ISectionViewModel>).FullName);
            Bind<IModelBinder>().To<ListModelBinder>().Named(typeof(IList<IFieldViewModel>).FullName);
            Bind<IModelBinder>().To<ListModelBinder>().Named(typeof(IList<SelectListItem>).FullName);
            Bind<IModelBinder>().To<FieldModelBinder>().Named(typeof(IFieldViewModel).FullName);
            Bind<IModelBinder>().To<UploadedFileModelBinder>().Named(typeof(UploadedFile).FullName);
            Bind<IModelBinder>().To<DateTimeModelBinder>().Named(typeof(DateTime?).FullName);
            Bind<IModelConverterService>().To<ModelConverterService>().InSingletonScope();

            // context
            Bind<IFlexContext>().To<FlexContext>().InRequestScope();

            // mailing
            Bind<IMailRepository>().To<MailRepository>();
            Bind<IMailService>().To<MailService>();

            // helpers
            Bind<IUrlService>().To<UrlService>().InSingletonScope();

            // third party classes
            // todo: all third party classes should have a constraint that they are only resolved when the calling assembly in Flex. The bottom constraint for ISitecoreContext only works when we don't use the service locator pattern (no manually resolving a class)
            Bind<IConfigurationManager>().To<ConfigurationManager>();

            Bind<ISitecoreContext>()
                .To<SitecoreContext>()
                .When(request => request.Target != null
                    && request.Target.Member.DeclaringType != null
                    && request.Target.Member.DeclaringType.FullName.StartsWith(Constants.RootNamespace))
                .WithConstructorArgument("contextName", Constants.GlassMapperContextName);
        }
    }
}
