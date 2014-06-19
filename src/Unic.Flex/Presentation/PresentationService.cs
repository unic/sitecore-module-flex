namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using Unic.Configuration;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Configuration.Extensions;

    public class PresentationService : IPresentationService
    {
        private const string ViewPath = "~/Views/Modules/Flex/{0}/{1}.cshtml";

        private const string DefaultTheme = "Default";
        
        private readonly IConfigurationManager configurationManager;

        public PresentationService(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }
        
        public string ResolveView(ControllerContext controllerContext, string viewName)
        {
            var specification = this.configurationManager.Get<PresentationConfiguration>(c => c.Theme);
            var theme = specification != null ? specification.Value : DefaultTheme;
            
            var themeView = string.Format(ViewPath, theme, viewName);
            if (this.ViewExists(controllerContext, themeView))
            {
                return themeView;
            }

            return string.Format(ViewPath, DefaultTheme, viewName);
        }

        private bool ViewExists(ControllerContext controllerContext, string path)
        {
            var viewResult = ViewEngines.Engines.FindView(controllerContext, path, null);
            return viewResult != null && viewResult.View != null;
        }
    }
}
