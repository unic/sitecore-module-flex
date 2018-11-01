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
            var marketingAutomationContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();
            return marketingAutomationContactService.GetContactValue(this.ContactFieldName?.Trim());
        }
    }
}