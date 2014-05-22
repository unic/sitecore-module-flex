﻿namespace Unic.Flex.ModelBinding
{
    using System;
    using System.Web.Mvc;
    using Unic.Flex.Context;
    using Unic.Flex.Model.ViewModel.Forms;

    public class FormModelBinder : DefaultModelBinder
    {
        private readonly IModelConverterService modelConverter;
        
        public FormModelBinder(IModelConverterService modelConverter)
        {
            this.modelConverter = modelConverter;
        }
        
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType != typeof(FormViewModel))
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            return this.modelConverter.ConvertToViewModel(FlexContext.Current.Form);
        }
    }
}