namespace Unic.Flex.Core.ModelBinding
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    /// <summary>
    /// Model binder for decimal values.
    /// </summary>
    public class DecimalModelBinder : DefaultModelBinder
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
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null) return base.BindModel(controllerContext, bindingContext);

            try
            {
                return Convert.ToDecimal(valueProviderResult.AttemptedValue, CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(decimal?);
            }
        }
    }
}
