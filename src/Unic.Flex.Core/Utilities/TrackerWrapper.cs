namespace Unic.Flex.Core.Utilities
{
    using Sitecore.Analytics;

    public class TrackerWrapper : ITrackerWrapper
    {
        public ITracker GetCurrentTracker()
        {
            if (Tracker.Current == null)
            {
                Tracker.StartTracking();
            }

            return Tracker.Current;
        }
    }
}
