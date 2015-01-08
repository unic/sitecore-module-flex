namespace Unic.Flex.Core.Plugs
{
    using Unic.Flex.Core.Context;

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
    }
}
