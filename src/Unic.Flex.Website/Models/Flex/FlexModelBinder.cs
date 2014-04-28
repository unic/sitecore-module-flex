using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unic.Flex.Website.Models.Flex
{
    using System.ComponentModel;
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.DomainModel.Steps;

    public class FlexModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType != typeof(FormViewModel))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }
            
            var form = FlexContext.Current.Form;

            var model = new FormViewModel();
            model.Title = model.Title;
            model.Introdcution = model.Introdcution;
            model.Step = form.GetActiveStep();

            return model;
        }
    }
}