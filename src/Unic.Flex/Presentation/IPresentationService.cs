namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Presentation service for all presentation related stuff.
    /// </summary>
    public interface IPresentationService
    {
        /// <summary>
        /// Resolves the complete view path to a given view name regarding the current theme.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="presentationComponent">The presentation component.</param>
        /// <returns>
        /// Complete path of the view
        /// </returns>
        string ResolveView(ControllerContext controllerContext, IPresentationComponent presentationComponent);
    }
}
