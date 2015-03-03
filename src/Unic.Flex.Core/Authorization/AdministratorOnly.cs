namespace Unic.Flex.Core.Authorization
{
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// Attribute to check if current logged in user in an administrator -> throw 401 if not
    /// </summary>
    public class AdministratorOnly : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Sitecore.Context.User.IsAdministrator)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
