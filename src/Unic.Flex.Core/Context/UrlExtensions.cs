namespace Unic.Flex.Core.Context
{
    using System;
    using System.Linq;
    using Sitecore;
    using DependencyInjection;
    using Model.Forms;
    using Model.Steps;

    /// <summary>
    /// Extensions for url related objects.
    /// </summary>
    public static class UrlExtensions
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// Url of the current item appended to the current form context
        /// </returns>
        public static string GetUrl(this IStep item, IFlexContext context)
        {
            if (context == null) return string.Empty;
            if (context.Item == null) return item.Url;
            if (context.Form == null) return item.Url;
            if (item.StepNumber == 1) return context.Item.Url;

            var honorTrailingSlash = Sitecore.Configuration.Settings.GetBoolSetting(Definitions.Constants.HonorTrailingSlashConfig, false);

            if (!honorTrailingSlash)
            {
                return string.Join("/", context.Item.Url, item.Url.Split('/').Last());
            }
            else
            {
                var contextItemUrl = StringUtil.RemovePostfix('/', context.Item.Url);
                return string.Join("/", contextItemUrl, item.Url.Split(new[]{ '/' }, StringSplitOptions.RemoveEmptyEntries).Last());
            }
        }

        /// <summary>
        /// Gets the first step URL.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>The url of the first step in the form</returns>
        public static string GetFirstStepUrl(this IForm form)
        {
            var context = DependencyResolver.Resolve<IFlexContext>();
            return context.Item == null ? string.Empty : context.Item.Url;
        }
    }
}
