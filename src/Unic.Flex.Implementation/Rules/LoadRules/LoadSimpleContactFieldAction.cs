namespace Unic.Flex.Implementation.Rules.LoadRules
{
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class LoadSimpleContactFieldAction<T> : BaseFlexLoadAction<T>
        where T : RuleContext
    {
        public string ContactFieldName { get; set; }

        protected override object GetValue()
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();
            return analyticsContactService.GetContactValue(this.ContactFieldName?.Trim());
        }
    }
}