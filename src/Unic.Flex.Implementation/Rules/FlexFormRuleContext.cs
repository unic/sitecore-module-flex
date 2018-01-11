namespace Unic.Flex.Implementation.Rules
{
    using Model.Forms;
    using Sitecore.Rules;

    public class FlexFormRuleContext : RuleContext
    {
        public IForm Form { get; set; }
    }
}