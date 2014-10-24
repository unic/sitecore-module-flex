namespace Unic.Flex.Analytics
{
    using Unic.Flex.Model.DomainModel;
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
        /// <param name="item">The item.</param>
        void RegisterGoal(Goal goal, ItemBase item);
    }
}
