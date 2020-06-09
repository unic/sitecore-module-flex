namespace Unic.Flex.Core.Plugs
{
    /// <summary>
    /// Service to manage server origin
    /// </summary>
    public interface IServerOriginService
    {
        /// <summary>
        /// Checks if Honoring origin of server is enabled, gets server origin from configuration
        /// </summary>
        string GetServerOrigin();

        /// <summary>
        /// Checks if Honoring origin of server is enabled
        /// </summary>
        bool IsServerOriginCheckEnabled();
    }
}
