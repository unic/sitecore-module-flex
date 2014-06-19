namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using Unic.Configuration;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Configuration.Extensions;

    public class PresentationService : IPresentationService
    {
        public string ResolveView(ControllerContext controllerContext, string viewName)
        {

            // todo: abstract this in a service

            var configManager = new ConfigurationManager();
            var specification = configManager.Get<PresentationConfiguration>(c => c.Theme);
            var theme = specification != null ? specification.Value : "undefined";
            
            // todo: make dynmic and/or move to config

            var themeView = "~/Views/Modules/Flex/" + theme + "/" + viewName + ".cshtml";
            if (this.ViewExists(controllerContext, themeView))
            {
                return themeView;
            }

            return "~/Views/Modules/Flex/Default/" + viewName + ".cshtml";
        }

        private bool ViewExists(ControllerContext controllerContext, string path)
        {
            var viewResult = ViewEngines.Engines.FindView(controllerContext, path, null);
            return viewResult != null && viewResult.View != null;
        }
    }
}
