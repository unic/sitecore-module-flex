namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;

    /// <summary>
    /// Presentation service for all presentation related stuff.
    /// </summary>
    public interface IPresentationService
    {
        /// <summary>
        /// Resolves the complete view path to a given view name regarding the current theme.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>Complete path of the view</returns>
        string ResolveView(ControllerContext controllerContext, string viewName);
    }
}
