namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class SaveSimpleContactFieldAction<T> : BaseFlexSaveAction<T> where T : RuleContext
    {
        public string ContactFieldName { get; set; }

        public override void Apply(T ruleContext)
        {
            var value = this.GetFieldValue(ruleContext);

            if (value == null) return;

            var marketingAutomationContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();
            marketingAutomationContactService.SetContactValue(this.ContactFieldName?.Trim(), value);
        }
    }
}