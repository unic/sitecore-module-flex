[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Unic.Flex.Website.FlexBootstrapper), "PostStart")]

namespace Unic.Flex.Website
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Sections;
    using Unic.Flex.ModelBinding;

    public class FlexBootstrapper
    {
        public static void PostStart()
        {
            RegisterModelBinders();
        }

        private static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(FormViewModel), Container.Kernel.Get<IModelBinder>());
            ModelBinders.Binders.Add(typeof(IList<SectionViewModel>), new ListModelBinder());
            ModelBinders.Binders.Add(typeof(IList<FieldViewModel>), new ListModelBinder());
        }
    }
}