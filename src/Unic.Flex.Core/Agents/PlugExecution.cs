namespace Unic.Flex.Core.Agents
{
    using Plugs;
    using DependencyResolver = Unic.Flex.Core.DependencyInjection.DependencyResolver;

    /// <summary>
    /// Agent for executing plugs
    /// </summary>
    public class PlugExecution
    {
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
        ///   <c>true</c> if it should log activity; otherwise, <c>false</c>.
        /// </value>
        public virtual bool LogActivity { get; set; }

        /// <summary>
        /// Gets or sets the log tag.
        /// </summary>
        /// <value>
        /// The log tag.
        /// </value>
        public virtual string LogTag { get; set; }

        /// <summary>
        /// Runs this agent.
        /// </summary>
        public virtual void Run()
        {
            var plugExecutionService = DependencyResolver.Resolve<IAsyncPlugExecutionService>();

            plugExecutionService.LogActivity = this.LogActivity;
            plugExecutionService.LogTag = this.LogTag;
            plugExecutionService.SiteName = this.SiteName;
            plugExecutionService.ExecutePlugs();
        }     
    }
}