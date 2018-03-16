namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class IdentifyContactEmailAction<T> : BaseFlexSaveAction<T>
        where T : RuleContext
    {

        public override void Apply(T ruleContext)
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            var value = this.GetFieldValue(ruleContext);
            if (value == null) return;

            analyticsContactService.IdentifyContactEmail( value);
        }
    }
}