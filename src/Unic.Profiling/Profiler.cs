namespace Unic.Profiling
{
    using System;

    /// <summary>
    /// Profiler to track specific events.
    /// </summary>
    public static class Profiler
    {
        /// <summary>
        /// Occurs when an event starts profiling.
        /// </summary>
        public static event EventHandler<ProfilingEventArgs> StartProfiling = delegate { };

        /// <summary>
        /// Occurs when an event ends profiling.
        /// </summary>
        public static event EventHandler<ProfilingEventArgs> EndProfiling = delegate { };

        /// <summary>
        /// Called when a profiling should be started.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventName">Name of the event.</param>
        public static void OnStart(object sender, string eventName)
        {
            StartProfiling(sender, new ProfilingEventArgs(eventName));
        }

        /// <summary>
        /// Called when a profiling should be ended.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventName">Name of the event.</param>
        public static void OnEnd(object sender, string eventName)
        {
            EndProfiling(sender, new ProfilingEventArgs(eventName));
        }
    }
}
