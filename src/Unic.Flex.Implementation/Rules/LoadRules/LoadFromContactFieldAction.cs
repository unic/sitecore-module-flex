namespace Unic.Flex.Implementation.Rules.LoadRules
{
    using System;
    using Sitecore.Rules;

    public class LoadFromContactFieldAction<T> : BaseFlexLoadAction<T>
        where T : RuleContext
    {
        protected override object GetValue()
        {
            throw new NotImplementedException();
        }
    }
}