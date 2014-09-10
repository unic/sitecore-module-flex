namespace Unic.Flex.Plugs
{
    using Unic.Flex.Context;

    /// <summary>
    /// Service for the plug framework
    /// </summary>
    public interface IPlugsService
    {
        /// <summary>
        /// Executes the load plugs.
        /// </summary>
        /// <param name="context">The context.</param>
        void ExecuteLoadPlugs(IFlexContext context);

        /// <summary>
        /// Executes the save plugs.
        /// </summary>
        /// <param name="context">The context.</param>
        void ExecuteSavePlugs(IFlexContext context);

        /// <summary>
        /// Executes the tasks.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        void ExecuteTasks(int sessionId = 0);
    }
}
