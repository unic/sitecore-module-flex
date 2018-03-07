namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using System;
    using Core.DependencyInjection;
    using Core.MarketingAutomation;
    using Sitecore.Rules;

    public class SaveLanguageAction<T> : BaseFlexSaveAction<T>
        where T : RuleContext
    {
        private readonly Guid languageField = Guid.Parse("{20076F81-D4D9-43FF-B79A-A255A3F8A9DF}");

        public override void Apply(T ruleContext)
        {
            var analyticsContactService = DependencyResolver.Resolve<IMarketingAutomationContactService>();

            var language = Sitecore.Context.Language.Name;

            analyticsContactService.SetContactValue(this.languageField, language);
        }
    }
}