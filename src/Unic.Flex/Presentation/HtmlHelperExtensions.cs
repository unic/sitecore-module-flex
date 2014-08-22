namespace Unic.Flex.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Unic.Flex.Definitions;
    using Unic.Flex.DependencyInjection;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Fields;

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
                PresentationService.Value.ResolveView(htmlHelper.ViewContext, model),
                model,
                new ViewDataDictionary(htmlHelper.ViewData)
                    {
                        TemplateInfo = new TemplateInfo
                        {
                            HtmlFieldPrefix = htmlHelper.GetName(propertyName)
                        }
                    });
        }

        /// <summary>
        /// Extension method for a label. This adds needed attributes for the frontend.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="labelText">The label text.</param>
        /// <returns>Html string with the markup for a label</returns>
        public static MvcHtmlString FlexLabel<TModel, TValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            string labelText)
        {
            return htmlHelper.LabelFor(
                expression,
                labelText,
                new
                    {
                        @class = Constants.LabelCssClass,
                        id = htmlHelper.GetId(Constants.LabelIdSuffix)
                    });
        }

        /// <summary>
        /// Gets the id of the current html field and add the suffix.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The generated id</returns>
        public static string GetId(this HtmlHelper htmlHelper, string suffix)
        {
            return htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(suffix);
        }

        /// <summary>
        /// Gets the name of the current html field and add the suffix.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The generated name</returns>
        public static string GetName(this HtmlHelper htmlHelper, string suffix)
        {
            return htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(suffix);
        }

        /// <summary>
        /// Adds the validator for a specific field
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Html string with the markup for the validator</returns>
        public static MvcHtmlString FlexValidator<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.ValidationMessageFor(
                expression,
                null,
                new
                    {
                        role = "alert",
                        aria_labelledby = htmlHelper.GetId(Constants.LabelIdSuffix)
                    });
        }

        /// <summary>
        /// Gets the attributes for a model and add context specific attributes.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns>Additional attributes for the html markup</returns>
        public static IDictionary<string, object> GetAttributes(this HtmlHelper htmlHelper, IFieldViewModel viewModel)
        {
            var attributes = viewModel.Attributes;
            attributes.Add("aria-labelledby", htmlHelper.GetId(Constants.LabelIdSuffix));

            if (viewModel.Tooltip != null && viewModel.Tooltip.ShowTooltip)
            {
                attributes.Add("aria-describedby", htmlHelper.GetId(Constants.TooltipIdSuffix));
            }

            return attributes;
        }

        public static string FormatAttributes(this HtmlHelper htmlHelper, IDictionary<string, object> attributes)
        {
            return attributes.Aggregate(new StringBuilder(), (sb, kvp) => sb.AppendFormat("{0}=\"{1}\" ", kvp.Key, kvp.Value)).ToString();
        }
    }
}
