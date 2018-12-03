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
            var marketingAutomationContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            var value = this.GetFieldValue(ruleContext);
            if (value == null) return;

            marketingAutomationContactService.SetEmailAddress(this.EmailAddressName, value);
        }

    }
}
