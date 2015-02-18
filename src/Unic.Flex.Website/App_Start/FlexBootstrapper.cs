[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.FlexBootstrapper), "PostStart")]

namespace Unic.Flex.Website.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Unic.Configuration.Converter;
    using Unic.Flex.Model.Configuration.Converters;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

    /// <summary>
    /// Bootstrapper for the Flex form module.
    /// </summary>
    public class FlexBootstrapper
    {
        /// <summary>
        /// The method called after starting the application.
        /// </summary>
        public static void PostStart()
        {
            RegisterModelBinders();
            RegisterConfigurationConverters();
        }

        /// <summary>
        /// Registers the ASP.NET MVC model binders.
        /// </summary>
        private static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(IFormViewModel), DependencyResolver.Resolve<IModelBinder>(typeof(IFormViewModel).FullName));
            ModelBinders.Binders.Add(typeof(IList<ISectionViewModel>), DependencyResolver.Resolve<IModelBinder>(typeof(IList<ISectionViewModel>).FullName));
            ModelBinders.Binders.Add(typeof(IList<IFieldViewModel>), DependencyResolver.Resolve<IModelBinder>(typeof(IList<IFieldViewModel>).FullName));
            ModelBinders.Binders.Add(typeof(IList<SelectListItem>), DependencyResolver.Resolve<IModelBinder>(typeof(IList<SelectListItem>).FullName));
            ModelBinders.Binders.Add(typeof(IFieldViewModel), DependencyResolver.Resolve<IModelBinder>(typeof(IFieldViewModel).FullName));
            ModelBinders.Binders.Add(typeof(UploadedFile), DependencyResolver.Resolve<IModelBinder>(typeof(UploadedFile).FullName));
            ModelBinders.Binders.Add(typeof(DateTime?), DependencyResolver.Resolve<IModelBinder>(typeof(DateTime?).FullName));
            ModelBinders.Binders.Add(typeof(decimal?), DependencyResolver.Resolve<IModelBinder>(typeof(decimal?).FullName));
        }

        /// <summary>
        /// Registers the configuration converters for the config module.
        /// </summary>
        private static void RegisterConfigurationConverters()
        {
            ConverterFactory.RegisterConverter(new SpecificationConverter());
        }
    }
}