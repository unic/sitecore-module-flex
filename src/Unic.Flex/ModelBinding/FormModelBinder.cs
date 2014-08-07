namespace Unic.Flex.ModelBinding
{
    using System;
    using System.Web.Mvc;
    using Ninject;
    using Unic.Flex.Context;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.ViewModel.Forms;

    public class FormModelBinder : DefaultModelBinder
    {
        private readonly IModelConverterService modelConverter;
        
        public FormModelBinder(IModelConverterService modelConverter)
        {
            this.modelConverter = modelConverter;
        }
        
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType != typeof(IFormViewModel))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            var context = Container.Kernel.Get<IFlexContext>();
            return this.modelConverter.ConvertToViewModel(context.Form);
        }
    }
}