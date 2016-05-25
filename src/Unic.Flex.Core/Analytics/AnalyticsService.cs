namespace Unic.Flex.Core.Analytics
{
    using System;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Data;
    using Unic.Flex.Core.Logging;
    using Unic.Flex.Model.Analytics;

    /// <summary>
    /// Analytics related actions.
    /// </summary>
    public class AnalyticsService : IAnalyticsService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AnalyticsService(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Registers the goal.
        /// </summary>
        /// <param name="goal">The goal.</param>
        /// <param name="itemId">The item identifier.</param>
        public virtual void RegisterGoal(Goal goal, Guid itemId)
        {
            // check if analytics is enabled
            if (!Tracker.IsActive)
            {
                this.logger.Info("Analytics is disabled, therefor the Goal has not been triggered", this);
                return;
            }

            // check if we have a valid page
            if (Tracker.Current == null
                || Tracker.Current.Session == null
                || Tracker.Current.Session.Interaction == null
                || Tracker.Current.Session.Interaction.CurrentPage == null)
            {
                this.logger.Error("No valid tracker or session available, therefor the Goal has not  been triggered", this);
                return;
            }

            // generate the data 
            var pageEventData = new PageEventData("Flex", goal.InnerItem.ID.ToGuid())
            {
                ItemId = itemId,
                Data = "Form submit completed",
                Text = goal.Description
            };

            // register the event
            Tracker.Current.Session.Interaction.CurrentPage.Register(pageEventData);
        }
    }
}
