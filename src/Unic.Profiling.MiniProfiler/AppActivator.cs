[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Unic.Profiling.MiniProfiler.AppActivator), "PreStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Unic.Profiling.MiniProfiler.AppActivator), "ApplicationShutdown")]

namespace Unic.Profiling.MiniProfiler
{
    using System.Configuration;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using StackExchange.Profiling;

    /// <summary>
    /// Activate the profiling events
    /// </summary>
    public class AppActivator
    {   
        /// <summary>
        /// Add profiling event listeners before application has been started.
        /// </summary>
        public static void PreStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));
            Profiling.Profiler.StartProfiling += Profiler.Start;
            Profiling.Profiler.EndProfiling += Profiler.End;
        }

        /// <summary>
        /// Remove profiling event listeners when application has been stopped.
        /// </summary>
        public static void ApplicationShutdown()
        {
            // remote event listeners
            Profiling.Profiler.StartProfiling -= Profiler.Start;
            Profiling.Profiler.EndProfiling -= Profiler.End;
        }

        /// <summary>
        /// Starts the Miniprofiler
        /// </summary>
        private class MiniProfilerStartupModule : IHttpModule
        {
            /// <summary>
            /// Gets a value indicating whether the MiniProfiler is enabled.
            /// Set the AppSettings value "Profiling.DisableMiniProfier" to "true" to disable it.
            /// By default, the MiniProfiler is enabled.
            /// </summary>
            /// <value>
            /// <c>true</c> if the MiniProfiler is enabled; otherwise, <c>false</c>.
            /// </value>
            private static bool IsEnabled
            {
                get
                {
                    var disabled = ConfigurationManager.AppSettings["Profiling.DisableMiniProfier"];
                    if (disabled == null) return true;

                    bool boolValue;
                    return !(bool.TryParse(disabled, out boolValue) && boolValue);
                }
            }

            /// <summary>
            /// Initializes a module and prepares it to handle requests.
            /// </summary>
            /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
            public void Init(HttpApplication context)
            {
                context.BeginRequest += (sender, e) => { if (IsEnabled) MiniProfiler.Start(); };
                context.EndRequest += (sender, e) => MiniProfiler.Stop();
            }

            /// <summary>
            /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}
