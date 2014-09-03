namespace Unic.Flex.ModelBinding
{
    using System;
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.ViewModel.Forms;

    /// <summary>
    /// Bind the form model.
    /// </summary>
    public class FormModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// The model converter
        /// </summary>
        private readonly IModelConverterService modelConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormModelBinder"/> class.
        /// </summary>
        /// <param name="modelConverter">The model converter.</param>
        public FormModelBinder(IModelConverterService modelConverter)
        {
            this.modelConverter = modelConverter;
        }

        /// <summary>
        /// Creates the specified model type by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <param name="modelType">The type of the model object to return.</param>
        /// <returns>
        /// A data object of the specified type.
        /// </returns>
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType != typeof(IFormViewModel))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            var context = Container.Resolve<IFlexContext>();
            context.ViewModel = this.modelConverter.ConvertToViewModel(context.Form);
            return context.ViewModel;
        }
    }
}