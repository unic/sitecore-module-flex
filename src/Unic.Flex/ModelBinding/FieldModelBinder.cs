using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.ModelBinding
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class FieldModelBinder : DefaultModelBinder
    {
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
