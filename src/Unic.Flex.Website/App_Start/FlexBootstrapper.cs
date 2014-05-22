[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.FlexBootstrapper), "PostStart")]

namespace Unic.Flex.Website
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Ninject;
    using Unic.Flex.DependencyInjection;
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
        }

        /// <summary>
        /// Registers the ASP.NET MVC model binders.
        /// </summary>
        private static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(FormViewModel), Container.Kernel.Get<IModelBinder>());
            ModelBinders.Binders.Add(typeof(IList<SectionViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<FieldViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(FieldViewModel), new ValueModelBinder());
        }
    }
}