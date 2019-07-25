namespace Unic.Flex.Core.ModelBinding
{
    using System.ComponentModel;
    using System.Web.Mvc;
    using Unic.Flex.Core.Definitions;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Types;
    using Utilities;

    /// <summary>
    /// Bind fields
    /// </summary>
    public class FieldModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// The initial value of the model
        /// </summary>
        private object initialValue;

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
            // if we do have a field without post values, directly return without binding
            if (bindingContext.Model is IFieldWithoutPost) return bindingContext.Model;
            
            // get initial values
            var initialModel = (IField)bindingContext.Model;
            this.initialValue = initialModel.Value;
            initialModel.ModelName = bindingContext.ModelName;

            // bind the model with the default model binder
            var model = base.BindModel(controllerContext, bindingContext);

            // return the binded model if it could be binded
            if (model != null) return model;

            // otherwise no values has been posted -> reset the field value, do validation and return the initial model
            initialModel.Value = null;
            MappingHelper.ForceFieldValidation(bindingContext, initialModel);
            return initialModel;
        }

        /// <summary>
        /// Binds the specified property by using the specified controller context and binding context and the specified property descriptor.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <param name="propertyDescriptor">Describes a property to be bound. The descriptor provides information such as the component type, property type, and property value. It also provides methods to get or set the property value.</param>
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.DisplayName == Constants.ValueIdSuffix)
            {
                var properties = TypeDescriptor.GetProperties(bindingContext.Model.GetType());
                propertyDescriptor = properties.Find(Constants.ValueIdSuffix, false);
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        /// <summary>
        /// Called when the model is updated.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);

            // set the file uploaded to the initial file (because it maybe wasn't posted but was posted before)
            var model = (IField)bindingContext.Model;
            if (model.Value == null && model.Type == typeof(UploadedFile))
            {
                model.Value = this.initialValue;
            }

            MappingHelper.ForceFieldValidation(bindingContext, model);
        }
    }
}
