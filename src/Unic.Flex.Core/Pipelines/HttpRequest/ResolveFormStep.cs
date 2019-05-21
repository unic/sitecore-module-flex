namespace Unic.Flex.Core.Pipelines.HttpRequest
{
    using System;
    using System.Linq;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.HttpRequest;
    using Unic.Flex.Core.Context;
    using Unic.Flex.Core.DependencyInjection;
    using Unic.Flex.Core.Logging;

    /// <summary>
    /// Pipeline processor for resolving the current form step.
    /// </summary>
    public class ResolveFormStep : HttpRequestProcessor
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Processes the current pipeline processor
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(HttpRequestArgs args)
        {
            this.logger = DependencyResolver.Resolve<ILogger>();

            // verify context
            if (Sitecore.Context.Database == null || Sitecore.Context.GetSiteName() == "shell" || Sitecore.Context.GetSiteName() == "login")
            {
                this.logger.Debug("Skipping form step resolving due to invalid context", this);
                return;
            }

            // inject classes
            var contextService = DependencyResolver.Resolve<IContextService>();
            var flexContext = DependencyResolver.Resolve<IFlexContext>();

            // resolve item
            var item = flexContext.Item != null ? Sitecore.Context.Database.GetItem(flexContext.Item.ItemId.ToString()) : null;
            var rewriteContextItem = false;

            // get the current path
            var path = args.LocalPath;
            if (path.EndsWith("/"))
            {
                path = path.Remove(path.Length - 1);
            }

            // load the parent item based on current path
            if (item == null && !string.IsNullOrWhiteSpace(path))
            {
                // resolve the parent item of the current path
                var parentPath = path.Contains("/") ? path.Remove(path.LastIndexOf('/')) : path;
                item = this.ResolveItem(parentPath);

                // we need to rewrite the context item at the end
                rewriteContextItem = true;
            }

            // do nothing if we have no item context
            if (item == null)
            {
                this.logger.Debug(string.Format("No context item found for path '{0}', exit form step resolving", args.LocalPath), this);
                return;
            }

            // get the current form included on the item
            var formDatasource = contextService.GetRenderingDatasource(item, Sitecore.Context.Device);
            if (string.IsNullOrWhiteSpace(formDatasource))
            {
                this.logger.Debug("No valid datasource for a Flex rendering found, exit form step resolving", this);
                return;
            }

            // load the form
            var form = contextService.LoadForm(formDatasource);
            if (form == null || !form.Steps.Any())
            {
                this.logger.Debug("Form is null or has no valid steps, exit form step resolving", this);
                return;
            }

            // save the form to the current items collection
            flexContext.Form = form;

            // if we are on the main step, everything is fine now
            if (!rewriteContextItem)
            {
                flexContext.Form.Steps.First().IsActive = true;
                return;
            }

            // check if we are on a valid step (/ means the first step and is valid)
            var currentUrlPart = path.Split('/').Last();
            var activeStep = form.Steps.Skip(1).FirstOrDefault(step => this.IsStepEqual(step.Url, currentUrlPart));
            if (activeStep == null)
            {
                this.logger.Debug("Something went wrong, current step could not be loaded by url, exit form step resolving", this);
                return;
            }

            // rewrite current step and context item if everything is ok
            activeStep.IsActive = true;
            flexContext.SetContextItem(item);
        }

        /// <summary>
        /// Resolves the item based on a url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// Item if found or null
        /// </returns>
        protected virtual Item ResolveItem(string url)
        {
            // get root item
            var startPath = Sitecore.Context.Site.StartPath;
            if (string.IsNullOrWhiteSpace(startPath)) return null;
            var item = Sitecore.Context.Database.GetItem(startPath);
            if (item == null) return null;

            // resolve item from path
            if (string.IsNullOrWhiteSpace(url)) return item;
            var urlParts = url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in urlParts)
            {
                item = this.ResolveChild(item, part);
                if (item == null) return null;
            }

            return item;
        }

        /// <summary>
        /// Resolves the child item for a given name/display name.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="name">The name.</param>
        /// <returns>Child item with name/display name if found</returns>
        private Item ResolveChild(Item item, string name)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNullOrEmpty(name, "name");
            return item.Axes.SelectSingleItem(string.Format("*[CompareCaseInsensitive(@@name, '{0}') or CompareCaseInsensitive(@__Display name, '{0}') or CompareCaseInsensitive(@@name, '{1}') or CompareCaseInsensitive(@__Display name, '{1}')]", name, Sitecore.MainUtil.DecodeName(name)));
        }

        /// <summary>
        /// Determines whether the given step url matches the current url part.
        /// </summary>
        /// <param name="stepUrl">The step URL.</param>
        /// <param name="currentUrlPart">The current URL part.</param>
        /// <returns>Boolean value if the url parts match or not</returns>
        private bool IsStepEqual(string stepUrl, string currentUrlPart)
        {
            var lastPart = stepUrl.Split(new[]{'/'}, StringSplitOptions.RemoveEmptyEntries).Last();
            if (lastPart.Contains("."))
            {
                lastPart = lastPart.Remove(lastPart.LastIndexOf(".", StringComparison.Ordinal));
            }

            return lastPart.Equals(currentUrlPart, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
