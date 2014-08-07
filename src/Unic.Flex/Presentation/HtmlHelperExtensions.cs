namespace Unic.Flex.Presentation
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Extension methods for the Mvc Html Helper
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// The presentation service
        /// </summary>
        private static readonly Lazy<IPresentationService> PresentationService;

        /// <summary>
        /// Initializes static members of the <see cref="HtmlHelperExtensions"/> class.
        /// </summary>
        static HtmlHelperExtensions()
        {
            PresentationService = new Lazy<IPresentationService>(Container.Resolve<IPresentationService>);
        }

        /// <summary>
        /// Generate the partial view for a Flex component, regardless the themes view etc..
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Html string with the markup for the partial view.</returns>
        public static MvcHtmlString FlexComponent<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression) where TProperty : IPresentationComponent
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = modelMetaData.Model as IPresentationComponent;
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            return htmlHelper.Partial(
                PresentationService.Value.ResolveView(htmlHelper.ViewContext, model.ViewName),
                model,
                new ViewDataDictionary(htmlHelper.ViewData)
                    {
                        TemplateInfo = new TemplateInfo
                        {
                            HtmlFieldPrefix = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName)
                        }
                    });
        }
    }
}
