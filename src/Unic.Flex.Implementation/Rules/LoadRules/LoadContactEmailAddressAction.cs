namespace Unic.Flex.Implementation.Rules.LoadRules
{
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class LoadContactEmailAddressAction<T> : BaseFlexLoadAction<T>
        where T : RuleContext
    {
        public string EmailAddressName { get; set; }

        protected override object GetValue()
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            return analyticsContactService.GetEmailAddress(this.EmailAddressName);
        }
    }
}
