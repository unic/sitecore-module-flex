namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class IdentifyContactEmailAction<T> : BaseFlexSaveAction<T>
        where T : RuleContext
    {
        public string EmailAddressName { get; set; }

        public override void Apply(T ruleContext)
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            analyticsContactService.IdentifyContactEmail(this.EmailAddressName);
        }
    }
}