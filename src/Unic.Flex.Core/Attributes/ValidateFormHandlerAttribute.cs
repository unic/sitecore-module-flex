namespace Unic.Flex.Core.Attributes
{
    using System.Reflection;
    using System.Web.Mvc;
    using Unic.Flex.Core.Definitions;

    /// <summary>
    /// Validates if the current action can be executed -> if this form has been posted
    /// </summary>
    public class ValidateFormHandlerAttribute : ActionMethodSelectorAttribute
    {
        /// <summary>
        /// Determines whether the current action is valid for this request.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <returns>Boolean value if request is valid</returns>
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var controller = controllerContext.HttpContext.Request.Form[Constants.FormHandlerControllerFieldName];
            var action = controllerContext.HttpContext.Request.Form[Constants.FormHandlerActionFieldName];

            return !string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action)
                   && controller == controllerContext.Controller.GetType().Name && methodInfo.Name == action;
        }
    }
}
