namespace Unic.Flex.Core.Plugs
{
    using Sitecore.Configuration;

    public class ServerOriginService : IServerOriginService
    {
        /// <summary>
        /// Checks if Honoring origin of server is enabled, gets server origin from configuration
        /// </summary>
        /// <returns>origin of server, null when checking disabled</returns>
        public string GetServerOrigin()
        {
            return this.IsServerOriginCheckEnabled() ? this.GetServerOriginSetting() : null;
        }

        /// <summary>
        /// Checks if Honoring origin of server is enabled
        /// </summary>
        /// <returns>True if origin check is enabled</returns>
        public bool IsServerOriginCheckEnabled()
        {
            return this.GetHonorServerOriginSetting();
        }

        private bool GetHonorServerOriginSetting()
        {
            return Settings.GetBoolSetting(Definitions.Constants.HonorJobOrigin, false);
        }

        private string GetServerOriginSetting()
        {
            return Settings.GetSetting(Definitions.Constants.ServerOrigin);
        }
    }
}
