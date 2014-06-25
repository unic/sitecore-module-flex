namespace Unic.Flex.ModelBinding
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Unic.Flex.Model.ViewModel.Fields;

    public class FieldModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var initialModel = bindingContext.Model;
            var model = base.BindModel(controllerContext, bindingContext);

            // return the binded model if is could be bindet
            if (model != null) return model;

            // otherwise no values has been posted -> reset the field value, to validation and return the initial model
            ((IFieldViewModel)initialModel).Value = null;
            ForceModelValidation(bindingContext);
            return initialModel;
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.OnModelUpdated(controllerContext, bindingContext);
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
