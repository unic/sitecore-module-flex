[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.FlexBootstrapper), "PostStart")]

namespace Unic.Flex.Website
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.ViewModel.Fields;
    using Unic.Flex.Model.ViewModel.Fields.ListFields;
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
        }

        /// <summary>
        /// Registers the ASP.NET MVC model binders.
        /// </summary>
        private static void RegisterModelBinders()
        {
            // todo: check if really all binders here are correct and needed
            
            ModelBinders.Binders.Add(typeof(FormViewModel), Container.Kernel.Get<IModelBinder>());
            ModelBinders.Binders.Add(typeof(IList<StandardSectionViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<IFieldViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<SelectListItem>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IFieldViewModel), new FieldModelBinder());
        }
    }
}