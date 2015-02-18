namespace Unic.Flex.Core.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Sitecore.Mvc.Presentation;
    using Unic.Flex.Core.Definitions;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Fields;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

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
            PresentationService = new Lazy<IPresentationService>(() => DependencyResolver.Resolve<IPresentationService>());
        }

        /// <summary>
        /// Generate the partial view for a Flex component, regardless the themes view etc.
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
        /// Generate the partial view for a model and specific view, regardless the themes view etc.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="theme">The theme.</param>
        /// <returns>Html string with the markup for the partial view.</returns>
        public static MvcHtmlString FlexComponent<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string viewName,
            string theme = "")
        {
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var model = modelMetaData.Model;
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            return htmlHelper.Partial(
                PresentationService.Value.ResolveView(htmlHelper.ViewContext, viewName, theme),
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
        /// <param name="labelAdditionText">The label addition text.</param>
        /// <returns>
        /// Html string with the markup for a label
        /// </returns>
        public static MvcHtmlString FlexLabel<TModel, TValue>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            string labelText,
            string labelAdditionText = "")
        {
            // add the label addition text
            if (!string.IsNullOrWhiteSpace(labelAdditionText))
            {
                labelText += string.Format(" {0}", labelAdditionText);
            }

            return MvcHtmlString.Create(HttpUtility.HtmlDecode(htmlHelper.LabelFor(
                expression,
                labelText,
                new
                    {
                        @class = Constants.LabelCssClass,
                        id = htmlHelper.GetId(Constants.LabelIdSuffix)
                    }).ToString()));
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
                        @class = "flex_fielderror",
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

        /// <summary>
        /// Formats the attributes to output as the html markup.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>string in the format "attribute=value attribute2=value2"</returns>
        public static string FormatAttributes(this HtmlHelper htmlHelper, IDictionary<string, object> attributes)
        {
            return attributes.Aggregate(new StringBuilder(), (sb, kvp) => sb.AppendFormat("{0}=\"{1}\" ", kvp.Key, kvp.Value)).ToString();
        }

        /// <summary>
        /// Get form handler with two additional hidden fields for the current controller/action.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>Html string with the hidden fields</returns>
        public static HtmlString FormHandler(this HtmlHelper htmlHelper)
        {
            // get controller and action
            var action = GetValueFromCurrentRendering("Controller Action");
            var controller = GetValueFromCurrentRendering("Controller");
            if (!string.IsNullOrWhiteSpace(controller) && controller.Contains(","))
            {
                controller = controller.Split(',').First().Split('.').Last().Trim();
            }

            if (!string.IsNullOrWhiteSpace(controller) && !controller.Contains("Controller"))
            {
                controller += "Controller";
            }
            
            // empty result
            if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
            {
                return new HtmlString(string.Empty);
            }

            // append the hidden fields
            return new HtmlString(string.Format(
                "{0}{1}",
                htmlHelper.Hidden(Constants.FormHandlerControllerFieldName, controller),
                htmlHelper.Hidden(Constants.FormHandlerActionFieldName, action)));
        }

        /// <summary>
        /// Gets the value from current rendering.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Value of specific field of the current rendering</returns>
        private static string GetValueFromCurrentRendering(string fieldName)
        {
            var context = RenderingContext.CurrentOrNull;
            if (context == null || context.Rendering == null) return string.Empty;

            var value = context.Rendering[fieldName];
            if (!string.IsNullOrWhiteSpace(value)) return value;

            var renderingItem = context.Rendering.RenderingItem;
            return renderingItem != null ? renderingItem.InnerItem[fieldName] : string.Empty;
        }
    }
}
