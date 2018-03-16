namespace Unic.Flex.Core.Utilities
{
    using Sitecore.Analytics;

    public static class TrackerWrapper
    {
        public static ITracker GetCurrentTracker()
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            return Tracker.Current;
        }
    }
}
