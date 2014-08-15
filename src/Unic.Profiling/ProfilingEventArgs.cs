namespace Unic.Profiling
{
    using System;

    /// <summary>
    /// Event arguments for profiling
    /// </summary>
    public class ProfilingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilingEventArgs"/> class.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        public ProfilingEventArgs(string eventName)
        {
            this.EventName = eventName;
        }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; }
    }
}
