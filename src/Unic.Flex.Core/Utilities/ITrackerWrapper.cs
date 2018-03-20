namespace Unic.Flex.Core.Utilities
{
    using Sitecore.Analytics;

    public interface ITrackerWrapper
    {
         ITracker GetCurrentTracker();
    }
}
