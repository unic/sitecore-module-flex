namespace Unic.Flex.Implementation.Rules.LoadRules
{
    using Sitecore.Analytics;
    using Sitecore.Rules;

    public class LoadContactIdAction<T> : BaseFlexLoadAction<T>
        where T : RuleContext
    {
        protected override object GetValue()
        {
            return Tracker.Current.Contact?.Identifiers?.Identifier;
        }
    }
}