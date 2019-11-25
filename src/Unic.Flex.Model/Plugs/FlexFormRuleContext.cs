namespace Unic.Flex.Model.Plugs
{
    using Model.Forms;
    using Sitecore.Rules;

    public class FlexFormRuleContext : RuleContext
    {
        public IForm Form { get; set; }
    }
}