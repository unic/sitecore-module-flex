namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;

    public class PresentationService : IPresentationService
    {
        public string ResolveView(ControllerContext controllerContext, string viewName)
        {
            if (this.ViewExists(controllerContext, "~/Views/Modules/Flex/Default/" + viewName + ".cshtml"))
            {
                return "~/Views/Modules/Flex/Default/" + viewName + ".cshtml";
            }

            return string.Empty;
        }

        private bool ViewExists(ControllerContext controllerContext, string path)
        {
            var viewResult = ViewEngines.Engines.FindView(controllerContext, path, null);
            return viewResult != null && viewResult.View != null;
        }
    }
}
