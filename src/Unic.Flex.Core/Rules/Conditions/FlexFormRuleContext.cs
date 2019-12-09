namespace Unic.Flex.Core.Rules.Conditions
{
    using Model.Forms;
    using Sitecore.Rules;

    public class FlexFormRuleContext : RuleContext
    {
        public IForm Form { get; set; }
    }
}