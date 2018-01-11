namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class SaveContactEmailAction<T> : BaseFlexSaveAction<T> 
        where T : RuleContext
    {
        public string EmailAddressName { get; set; }

        public override void Apply(T ruleContext)
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            var value = this.GetFieldValue(ruleContext);
            if (value == null) return;

            analyticsContactService.SetEmailAddress(this.EmailAddressName, value);
        }

    }
}
