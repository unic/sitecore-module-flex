﻿namespace Unic.Flex.Core.ModelBinding
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Model.Validators;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.Mapping;
    using Unic.Flex.Core.Utilities;
    using Unic.Flex.Model.Forms;
    using Unic.Flex.Model.Types;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

    /// <summary>
    /// Bind the form model.
    /// </summary>
    public class FormModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// The user data repository
        /// </summary>
        private readonly IUserDataRepository userDataRepository;

        /// <summary>
        /// The view mapper
        /// </summary>
        private readonly IViewMapper viewMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormModelBinder" /> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="viewMapper">The view mapper.</param>
        public FormModelBinder(IUserDataRepository userDataRepository, IViewMapper viewMapper)
        {
            this.viewMapper = viewMapper;
            this.userDataRepository = userDataRepository;
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
            var form = model as IForm;
            if (form != null)
            {
                var allFields = form.ActiveStep.Sections.SelectMany(s => s.Fields).ToList();

                foreach (var field in allFields)
                {
                    MappingHelper.ForceFieldValidation(bindingContext, field, ValidationType.FormValidation);
                }

                // remove validation errors for not visible fields (due to field dependecy)
                for (var sectionIndex = 0; sectionIndex < form.ActiveStep.Sections.Count; sectionIndex++)
                {
                    var section = form.ActiveStep.Sections[sectionIndex];
                    for (var fieldIndex = 0; fieldIndex < section.Fields.Count; fieldIndex++)
                    {
                        var field = section.Fields[fieldIndex];
                        if (field.IsHidden)
                        {
                            // model state contains not only the state of the hidden field, but also its dependent fields
                            var fieldKey = MappingHelper.GetFormFieldIdPrefix(sectionIndex, fieldIndex);
                            bindingContext.ModelState.Keys.Where(key => key.StartsWith(fieldKey))
                                .ToList()
                                .ForEach(key => bindingContext.ModelState.Remove(key));
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
            if (modelType != typeof(IForm))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            var context = DependencyResolver.Resolve<IFlexContext>();
            this.viewMapper.Map(context);
            return context.Form;
        }
    }
}