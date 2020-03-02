namespace Unic.Flex.Core.Context
{
    using System;
    using System.Linq;
    using Sitecore;
    using DependencyInjection;
    using Glass.Mapper.Sc.Fields;
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

            var honorTrailingSlash = Sitecore.Configuration.Settings.GetBoolSetting(Definitions.Constants.HonorTrailingSlashConfig, false);

            if (context.Item == null) return HandleTrailingSlash(item.Url, honorTrailingSlash);
            if (context.Form == null) return HandleTrailingSlash(item.Url, honorTrailingSlash);
            if (item.StepNumber == 1) return HandleTrailingSlash(context.Item.Url, honorTrailingSlash);

            var lastUrlSegment = item.Url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
            var contextItemUrl = StringUtil.RemovePostfix('/', context.Item.Url);

            return HandleTrailingSlash(string.Join("/", contextItemUrl, lastUrlSegment), honorTrailingSlash);
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

        public static string BuildUrlWithQueryString(this Link link)
        {
            if (link == null) return string.Empty;

            var honorTrailingSlash = Sitecore.Configuration.Settings.GetBoolSetting(Definitions.Constants.HonorTrailingSlashConfig, false);

            var anchor = string.Empty;
            
            if (!string.IsNullOrWhiteSpace(link.Anchor))
                anchor = $"#{link.Anchor}";



            if (!honorTrailingSlash)
                return $"{link.Url}?{link.Query}{anchor}";

            return $"{StringUtil.RemovePostfix('/', link.Url)}/?{link.Query}{anchor}";
        }

        private static string HandleTrailingSlash(string url, bool honorTrailingSlash)
        {
            if (!honorTrailingSlash)
                return url;

            return StringUtil.RemovePostfix('/', url) + "/";
        }
    }
}
