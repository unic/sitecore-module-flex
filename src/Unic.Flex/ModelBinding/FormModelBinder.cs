namespace Unic.Flex.ModelBinding
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Mapping;
    using Unic.Flex.Model.ViewModel.Forms;
    using Unic.Flex.Utilities;
    using Unic.Profiling;

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
        /// The field dependency service
        /// </summary>
        private readonly IFieldDependencyService fieldDependencyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormModelBinder" /> class.
        /// </summary>
        /// <param name="modelConverter">The model converter.</param>
        /// <param name="fieldDependencyService">The field dependency service.</param>
        public FormModelBinder(IModelConverterService modelConverter, IFieldDependencyService fieldDependencyService)
        {
            this.modelConverter = modelConverter;
            this.fieldDependencyService = fieldDependencyService;
        }

        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            var form = model as IFormViewModel;
            if (form != null)
            {
                Profiler.OnStart(this, "Flex :: Removing validation errors for hidden fields (due to field dependency)");

                var allFields = form.Step.Sections.SelectMany(s => s.Fields).ToList();
                for (var sectionIndex = 0; sectionIndex < form.Step.Sections.Count; sectionIndex++)
                {
                    var section = form.Step.Sections[sectionIndex];
                    for (var fieldIndex = 0; fieldIndex < section.Fields.Count; fieldIndex++)
                    {
                        var field = section.Fields[fieldIndex];
                        if (string.IsNullOrWhiteSpace(field.DependentFrom)) continue;

                        if (!this.fieldDependencyService.IsDependentFieldVisible(allFields, field))
                        {
                            bindingContext.ModelState.Remove(MappingHelper.GetFormFieldId(sectionIndex, fieldIndex));
                        }
                    }
                }

                Profiler.OnEnd(this, "Flex :: Removing validation errors for hidden fields (due to field dependency)");
            }

            return form;
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