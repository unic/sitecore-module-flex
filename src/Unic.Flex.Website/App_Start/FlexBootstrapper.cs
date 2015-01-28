[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.FlexBootstrapper), "PostStart")]

namespace Unic.Flex.Website.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Unic.Configuration.Converter;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Model.Configuration.Converters;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;

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
            ModelBinders.Binders.Add(typeof(IFormViewModel), Container.Resolve<IModelBinder>(typeof(IFormViewModel).FullName));
            ModelBinders.Binders.Add(typeof(IList<ISectionViewModel>), Container.Resolve<IModelBinder>(typeof(IList<ISectionViewModel>).FullName));
            ModelBinders.Binders.Add(typeof(IList<IFieldViewModel>), Container.Resolve<IModelBinder>(typeof(IList<IFieldViewModel>).FullName));
            ModelBinders.Binders.Add(typeof(IList<SelectListItem>), Container.Resolve<IModelBinder>(typeof(IList<SelectListItem>).FullName));
            ModelBinders.Binders.Add(typeof(IFieldViewModel), Container.Resolve<IModelBinder>(typeof(IFieldViewModel).FullName));
            ModelBinders.Binders.Add(typeof(UploadedFile), Container.Resolve<IModelBinder>(typeof(UploadedFile).FullName));
            ModelBinders.Binders.Add(typeof(DateTime?), Container.Resolve<IModelBinder>(typeof(DateTime?).FullName));
            ModelBinders.Binders.Add(typeof(decimal?), Container.Resolve<IModelBinder>(typeof(decimal?).FullName));
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