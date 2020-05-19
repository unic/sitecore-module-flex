using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unic.Configuration.Core;
using Unic.Flex.Core.Utilities;
using Unic.Flex.Model.Configuration;
using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

namespace Unic.Flex.Core.Plugs
{
    public class AsyncPlugExecutionService : IAsyncPlugExecutionService
    {
        /// <summary>
        /// The log tag
        /// </summary>
        private string logTag;

        /// <summary>
        /// Gets or sets the name of the site.
        /// </summary>
        /// <value>
        /// The name of the site.
        /// </value>
        public virtual string SiteName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity should be logged.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it should log acticity; otherwise, <c>false</c>.
        /// </value>
        public virtual bool LogActivity { get; set; }

        /// <summary>
        /// Gets or sets the log tag.
        /// </summary>
        /// <value>
        /// The log tag.
        /// </value>
        public virtual string LogTag
        {
            get
            {
                if (!string.IsNullOrEmpty(this.logTag))
                {
                    return string.Format("Flex :: {0} :: ", this.logTag);
                }

                return string.Empty;
            }

            set
            {
                this.logTag = value;
            }
        }

        public void ExecutePlugs()
        {
            try
            {
                this.LogInfo("Starting async plug execution");
                var currentTimestamp = DateTime.Now;

                // get some infos
                this.LogInfo("--------------------------");
                this.LogInfo("Information");
                this.LogInfo("--------------------------");
                this.LogInfo(string.Format("Timestamp: {0}", currentTimestamp));
                this.LogInfo(string.Format("Site Name: {0}", this.SiteName));
                this.LogInfo("==========================");

                var siteContext = Factory.GetSite(this.SiteName);
                if (siteContext == null) throw new Exception(string.Format("Invalid site name '{0}'", this.SiteName));

                using (new SiteContextSwitcher(siteContext))
                {
                    // check if async execution is allowed
                    var isAsyncExecutionAllowed = DependencyResolver.Resolve<IConfigurationManager>().Get<GlobalConfiguration>(c => c.IsAsyncExecutionAllowed);

                    // show config
                    this.LogInfo("--------------------------");
                    this.LogInfo("Configuration");
                    this.LogInfo("--------------------------");
                    this.LogInfo(string.Format("Is Async Execution Allowed: {0}", isAsyncExecutionAllowed));
                    this.LogInfo("==========================");

                    // only executing anything if async execution is allowed
                    if (isAsyncExecutionAllowed)
                    {
                        // generate web request to execute the plugs. This is needed because we need a HttpContext to execute the plugs
                        var url = this.GetControllerUrl(siteContext.Name);
                        this.LogInfo("Call controller url to execute plugs: " + url);
                        (new WebClient()).DownloadString(url);
                    }
                    else
                    {
                        this.LogInfo("Async execution of plugs is disabled, therefor do nothing...");
                    }
                }
            }
            catch (Exception exception)
            {
                this.LogError("Error while executing async plug execution", exception);
            }
        }


        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void LogInfo(string message)
        {
            if (!this.LogActivity)
            {
                return;
            }

            Log.Info(this.LogTag + message, this);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        protected virtual void LogError(string message, Exception exception)
        {
            if (!this.LogActivity)
            {
                return;
            }

            Log.Error(this.LogTag + message, exception, this);
        }

        /// <summary>
        /// Gets the controller URL neeed for executing the plugs with a HttpContext.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <returns>Url to request</returns>
        private string GetControllerUrl(string siteName)
        {
            var now = DateUtil.IsoNow;
            var urlHelper = this.GetUrlHelper();
            return WebUtil.GetFullUrl(
                    urlHelper.RouteUrl(
                        Model.Constants.MvcRouteName,
                        new { controller = "Flex", action = "ExecutePlugs", sc_site = siteName, timestamp = now, hash = SecurityUtil.GetMd5Hash(MD5.Create(), now) }),
                    WebUtil.GetServerUrl());
        }

        /// <summary>
        /// Gets the URL helper.
        /// </summary>
        /// <returns>Generated instance of the url helper</returns>
        private UrlHelper GetUrlHelper()
        {
            var request = new HttpRequest("/", WebUtil.GetServerUrl(), string.Empty);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);

            return new UrlHelper(requestContext);
        }
    }
}
