namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;

    public interface IPresentationService
    {
        string ResolveView(ControllerContext controllerContext, string viewName);
    }
}
