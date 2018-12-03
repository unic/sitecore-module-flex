namespace Unic.Flex.Implementation.Rules.LoadRules
{
    using System;
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class LoadFromContactFieldAction<T> : BaseFlexLoadAction<T>
        where T : RuleContext
    {
        public Guid ContactField { get; set; }

        protected override object GetValue()
        {
            var marketingAutomationContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();
            return marketingAutomationContactService.GetContactValue(this.ContactField);
        }
    }
}