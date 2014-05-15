namespace Unic.Flex.Website.ModelBinding
{
    using System;
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Website.Models.Flex;

    public class FormModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType != typeof(FormViewModel))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            return FlexContext.Current.Form.ToViewModel();
        }
    }
}