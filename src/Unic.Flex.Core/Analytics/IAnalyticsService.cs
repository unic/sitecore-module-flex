namespace Unic.Flex.Core.Analytics
{
    using System;
    using Unic.Flex.Model.DomainModel.Analytics;

    /// <summary>
    /// Analytics specific actions.
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Registers the goal.
        /// </summary>
        /// <param name="goal">The goal.</param>
        /// <param name="itemId">The item identifier.</param>
        void RegisterGoal(Goal goal, Guid itemId);
    }
}
