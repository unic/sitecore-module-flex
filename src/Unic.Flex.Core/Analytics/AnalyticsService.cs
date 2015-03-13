namespace Unic.Flex.Core.Analytics
{
    using System;
    using Sitecore.Analytics.Data.Items;
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
            if (!Sitecore.Analytics.Tracker.IsActive)
            {
                this.logger.Info("Analytics is disabled, therefor the Goal has not been triggered", this);
                return;
            }

            // check if we have a valid page
            if (Sitecore.Analytics.Tracker.CurrentPage == null)
            {
                this.logger.Error("Analytics.CurrentPage is null, therefor the Goal has not  been triggered", this);
                return;
            }
            
            // register the goal
            var pageEventRow = Sitecore.Analytics.Tracker.CurrentPage.Register(new PageEventItem(goal.InnerItem));
            pageEventRow.DataKey = "Flex";
            pageEventRow.Data = "Form submit completed";
            pageEventRow.Text = goal.Description;
            pageEventRow.ItemId = itemId;
            Sitecore.Analytics.Tracker.Submit();
        }
    }
}
