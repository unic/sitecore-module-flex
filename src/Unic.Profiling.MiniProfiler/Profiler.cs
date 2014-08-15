namespace Unic.Profiling.MiniProfiler
{
    using System;
    using System.Collections.Generic;
    using StackExchange.Profiling;

    /// <summary>
    /// Profiler for the Mini Profiler to profile the specific events
    /// </summary>
    public static class Profiler
    {
        /// <summary>
        /// The locker object
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// The profiles
        /// </summary>
        private static readonly IDictionary<string, IDisposable> Profiles = new Dictionary<string, IDisposable>();

        /// <summary>
        /// Starts profiling a specific event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ProfilingEventArgs"/> instance containing the event data.</param>
        public static void Start(object sender, ProfilingEventArgs e)
        {
            var profiler = MiniProfiler.Current;
            if (profiler == null) return;
            
            lock (Locker)
            {
                Profiles.Add(e.EventName, MiniProfiler.Current.Step(e.EventName));
            }
        }

        /// <summary>
        /// Ends profiling of the specific event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ProfilingEventArgs"/> instance containing the event data.</param>
        public static void End(object sender, ProfilingEventArgs e)
        {
            lock (Locker)
            {
                if (Profiles.ContainsKey(e.EventName))
                {
                    Profiles[e.EventName].Dispose();
                    Profiles.Remove(e.EventName);
                }
            }
        }
    }
}
