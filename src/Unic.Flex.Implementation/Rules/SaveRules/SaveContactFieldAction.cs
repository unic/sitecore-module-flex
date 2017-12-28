namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using System;
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class SaveContactFieldAction<T> : BaseFlexSaveAction<T>
        where T : RuleContext
    {
        public Guid ContactField { get; set; }

        public override void Apply(T ruleContext)
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            var value = this.GetFieldValue(ruleContext);
            if (value == null) return;

            analyticsContactService.SetContactValue(this.ContactField, value);
        }
    }
}