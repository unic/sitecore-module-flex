namespace Unic.Flex.Presentation
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Ninject;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Presentation;

    public static class HtmlHelperExtensions
    {
        private static readonly Lazy<IPresentationService> presentationService;

        static HtmlHelperExtensions()
        {
            presentationService = new Lazy<IPresentationService>(() => Container.Kernel.Get<IPresentationService>());
        }

        public static MvcHtmlString FlexComponent<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression) where TProperty : IPresentationComponent
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = modelMetaData.Model as IPresentationComponent;
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            return htmlHelper.Partial(
                presentationService.Value.ResolveView(htmlHelper.ViewContext, model.ViewName),
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
