namespace Unic.Flex.Core.Plugs
{
    /// <summary>
    /// Service for async plug execution
    /// </summary>
    public interface IAsyncPlugExecutionService
    {
        /// <summary>
        /// Gets or sets the name of the site.
        /// </summary>
        /// <value>
        /// The name of the site.
        /// </value>
        string SiteName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity should be logged.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it should log activity; otherwise, <c>false</c>.
        /// </value>
        bool LogActivity { get; set; }

        /// <summary>
        /// Gets or sets the log tag.
        /// </summary>
        /// <value>
        /// The log tag.
        /// </value>
        string LogTag { get; set; }

        /// <summary>
        /// Executes plugs by web request
        /// </summary>
        void ExecutePlugs();
    }
}
