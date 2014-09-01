namespace Unic.Flex.ModelBinding
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using Unic.Flex.Model.ViewModel.Fields;

    public class FieldModelBinder : DefaultModelBinder
    {
        private object initialValue;
        
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // get initial values
            var initialModel = (IFieldViewModel)bindingContext.Model;
            this.initialValue = initialModel.Value;

            // bind the model with the default model binder
            var model = base.BindModel(controllerContext, bindingContext);

            // return the binded model if it could be binded
            if (model != null) return model;

            // otherwise no values has been posted -> reset the field value, to validation and return the initial model
            initialModel.Value = null;
            ForceModelValidation(bindingContext);
            return initialModel;
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);

            // set the file uploaded to the initial file (because it maybe wasn't posted but was posted before)
            var model = (IFieldViewModel)bindingContext.Model;
            if (model.Value == null && model.Type == typeof(HttpPostedFileBase))
            {
                model.Value = this.initialValue;
            }

            ForceModelValidation(bindingContext);
        }

        private static void ForceModelValidation(ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model as IValidatableObject;
            if (model == null) return;

            var modelName = bindingContext.ModelName;
            var modelState = bindingContext.ModelState;
            var errors = model.Validate(new ValidationContext(model, null, null));
            foreach (var error in errors)
            {
                foreach (var memberName in error.MemberNames)
                {
                    modelState.AddModelError(string.Join(".", modelName, memberName), error.ErrorMessage);
                }
            }
        }
    }
}
