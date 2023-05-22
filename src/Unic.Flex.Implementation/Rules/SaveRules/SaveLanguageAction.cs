namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using System;
    using Sitecore.Rules;

    public class SaveLanguageAction<T> : BaseFlexSaveAction<T>
        where T : RuleContext
    {
        public override void Apply(T ruleContext)
        {
            throw new NotImplementedException();
        }
    }
}