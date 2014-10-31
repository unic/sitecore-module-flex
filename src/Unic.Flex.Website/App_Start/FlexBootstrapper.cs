[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.App_Start.FlexBootstrapper), "PostStart")]

namespace Unic.Flex.Website.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Unic.Configuration.Converter;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Configuration.Converters;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Model.ViewModel.Sections;
    using Unic.Flex.ModelBinding;

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
            ModelBinders.Binders.Add(typeof(IFormViewModel), Container.Resolve<IModelBinder>(typeof(FormModelBinder).FullName));
            ModelBinders.Binders.Add(typeof(IList<ISectionViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<IFieldViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<SelectListItem>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IFieldViewModel), new FieldModelBinder());
            ModelBinders.Binders.Add(typeof(UploadedFile), new UploadedFileModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), Container.Resolve<IModelBinder>(typeof(DateTimeModelBinder).FullName));
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