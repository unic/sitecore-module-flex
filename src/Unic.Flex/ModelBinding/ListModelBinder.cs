namespace Unic.Flex.ModelBinding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Unic.Flex.Model.ViewModel.Fields.ListFields;

    public class ListModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model;
            var collectionBindingContext = new ModelBindingContext
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, bindingContext.ModelType),
                ModelName = bindingContext.ModelName,
                ModelState = bindingContext.ModelState,
                PropertyFilter = bindingContext.PropertyFilter,
                ValueProvider = bindingContext.ValueProvider
            };

            return this.UpdateCollection(controllerContext, collectionBindingContext, model.GetType().GetGenericArguments()[0]);
        }

        private object UpdateCollection(ControllerContext controllerContext, ModelBindingContext bindingContext, Type elementType)
        {
            var collection = (IList)bindingContext.Model;
            var elementBinder = Binders.GetBinder(elementType);
            var modelList = new List<object>();

            for (var index = 0; index < collection.Count; index++)
            {
                // get the item and the type
                var item = collection[index];

                // todo: this is a really ugly hack... we need to find another solution for this...
                // the problem ist, that in the checkboxlist the value of each checkbox is binded to the .Selected property of each
                // value in the .Items<SelectListItem> property. Normally the elementType here is "IFieldViewModel" and this doesn't
                // contain a property .Items, so the items and the selected value will not be binded
                var innerType = item is ListFieldViewModel<string[]> ? typeof(ListFieldViewModel<string[]>) : elementType;
                
                // the change to the original model binder here is, that the model is taken from "collection[index]"
                // instead of "null" -> this way the collection element is not initialized new, the instance that
                // was in the list originally is taken instead
                var innerContext = new ModelBindingContext
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, innerType),
                    ModelName = CreateSubIndexName(bindingContext.ModelName, index),
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };
                
                modelList.Add(elementBinder.BindModel(controllerContext, innerContext));
            }

            return modelList;
        }
    }
}
