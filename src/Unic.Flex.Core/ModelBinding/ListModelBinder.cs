namespace Unic.Flex.Core.ModelBinding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Bind lists to the model.
    /// </summary>
    public class ListModelBinder : DefaultModelBinder
    {
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

        /// <summary>
        /// Updates the collection and keep existing instances.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="elementType">Type of the element.</param>
        /// <returns>The model object</returns>
        private object UpdateCollection(ControllerContext controllerContext, ModelBindingContext bindingContext, Type elementType)
        {
            var collection = (IList)bindingContext.Model;
            var elementBinder = this.Binders.GetBinder(elementType);
            var modelList = new List<object>();

            for (var index = 0; index < collection.Count; index++)
            {
                // get the item and the type
                var item = collection[index];

                // the change to the original model binder here is, that the model is taken from "collection[index]"
                // instead of "null" -> this way the collection element is not initialized new, the instance that
                // was in the list originally is taken instead
                var innerContext = new ModelBindingContext
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, elementType),
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
