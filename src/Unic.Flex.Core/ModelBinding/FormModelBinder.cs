namespace Unic.Flex.Core.ModelBinding
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Core.Utilities;
    using Unic.Flex.Model.Types;
    using Unic.Flex.Model.ViewModel.Forms;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

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
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormModelBinder" /> class.
        /// </summary>
        /// <param name="modelConverter">The model converter.</param>
        /// <param name="fieldDependencyService">The field dependency service.</param>
        /// <param name="userDataRepository">The user data repository.</param>
        public FormModelBinder(IModelConverterService modelConverter, IFieldDependencyService fieldDependencyService, IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
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
                var allFields = form.Step.Sections.SelectMany(s => s.Fields).ToList();

                // remove validation errors for not visible fields (due to field dependecy)
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

                // remove posted files if there are validation errors
                if (!bindingContext.ModelState.IsValid)
                {
                    foreach (var field in allFields.Where(field => field.Value != null && field.Type == typeof(UploadedFile)))
                    {
                        var sessionValue = this.userDataRepository.GetValue(form.Id, field.Id);
                        if (sessionValue != null && sessionValue.Equals(field.Value)) continue;

                        field.Value = null;
                        this.userDataRepository.RemoveValue(form.Id, field.Id);
                    }
                }
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

            var context = DependencyResolver.Resolve<IFlexContext>();
            context.ViewModel = this.modelConverter.ConvertToViewModel(context.Form);
            return context.ViewModel;
        }
    }
}