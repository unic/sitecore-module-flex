namespace Unic.Flex.Core.DependencyInjection
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
            this.Bind<IModelConverterService>().To<ModelConverterService>().InSingletonScope();

            // context
            this.Bind<IFlexContext>().To<FlexContext>().InRequestScope();

            // mailing
            this.Bind<IMailRepository>().To<MailRepository>();
            this.Bind<IMailService>().To<MailService>();

            // helpers
            this.Bind<IUrlService>().To<UrlService>().InSingletonScope();

            // third party classes
            this.Bind<IConfigurationManager>().To<ConfigurationManager>().When(this.GetFlexCondition());
            this.Bind<ISitecoreContext>().To<SitecoreContext>().When(this.GetFlexCondition()).WithConstructorArgument("contextName", Constants.GlassMapperContextName);
        }

        /// <summary>
        /// Gets the flex condition to only inject classes requested from a Flex class.
        /// </summary>
        /// <returns>Lamda expression with the condition</returns>
        protected virtual Func<IRequest, bool> GetFlexCondition()
        {
            return request =>
                request.Parameters.Contains(Container.FlexParameter)
                || (request.Target != null && request.Target.Member.DeclaringType != null
                    && request.Target.Member.DeclaringType.FullName.StartsWith(Constants.RootNamespace));
        }
    }
}
