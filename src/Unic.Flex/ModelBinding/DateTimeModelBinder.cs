namespace Unic.Flex.ModelBinding
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using Unic.Flex.Globalization;

    /// <summary>
    /// Model binder for datetime fields.
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// The culture service
        /// </summary>
        private readonly ICultureService cultureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeModelBinder"/> class.
        /// </summary>
        /// <param name="cultureService">The culture service.</param>
        public DateTimeModelBinder(ICultureService cultureService)
        {
            this.cultureService = cultureService;
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // get the value
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null) return null;

            // set the culture
            var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.DateTimeFormat.ShortDatePattern = this.cultureService.GetDateFormat();

            // set the new modelstate
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);

            try
            {
                return value.ConvertTo(typeof(DateTime), cultureInfo);
            }
            catch
            {
                return null;
            }
        }
    }
}
