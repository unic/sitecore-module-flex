namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using System;
    using Core.Context;
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class SaveLanguageAction<T> : BaseFlexSaveAction<T>
        where T : RuleContext
    {
        private static readonly Guid LanguageField = Guid.Parse("{20076F81-D4D9-43FF-B79A-A255A3F8A9DF}");

        public override void Apply(T ruleContext)
        {
            var marketingAutomationContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();
            var flexContext = DependencyResolver.Resolve<IFlexContext>();

            var language = flexContext.LanguageName;

            marketingAutomationContactService.SetContactValue(LanguageField, language);
        }
    }
}