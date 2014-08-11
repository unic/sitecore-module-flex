namespace Unic.Flex.Presentation
{
    using System.Web.Mvc;
    using Sitecore.Diagnostics;
    using Unic.Configuration;
    using Unic.Flex.Model.Configuration;
    using Unic.Flex.Model.Configuration.Extensions;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Presentation service implementation.
    /// </summary>
    public class PresentationService : IPresentationService
    {
        /// <summary>
        /// The view path pattern for razow views.
        /// </summary>
        private const string ViewPath = "~/Views/Modules/Flex/{0}/{1}.cshtml";

        /// <summary>
        /// The default theme
        /// </summary>
        private const string DefaultTheme = "Default";

        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationService"/> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        public PresentationService(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Resolves the complete view path to a given view name regarding the current theme.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="presentationComponent">The presentation component.</param>
        /// <returns>
        /// Complete path of the view
        /// </returns>
        public string ResolveView(ControllerContext controllerContext, IPresentationComponent presentationComponent)
        {
            Assert.ArgumentNotNull(presentationComponent, "presentationComponent");
            
            var specification = this.configurationManager.Get<PresentationConfiguration>(c => c.Theme);
            var theme = specification != null ? specification.Value : DefaultTheme;

            var themeView = string.Format(ViewPath, theme, presentationComponent.ViewName);
            if (this.ViewExists(controllerContext, themeView))
            {
                return themeView;
            }

            return string.Format(ViewPath, DefaultTheme, presentationComponent.ViewName);
        }

        /// <summary>
        /// Check if a view exists.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="path">The path.</param>
        /// <returns>Boolean value if the view exists or not.</returns>
        private bool ViewExists(ControllerContext controllerContext, string path)
        {
            var viewResult = ViewEngines.Engines.FindView(controllerContext, path, null);
            return viewResult != null && viewResult.View != null;
        }
    }
}
