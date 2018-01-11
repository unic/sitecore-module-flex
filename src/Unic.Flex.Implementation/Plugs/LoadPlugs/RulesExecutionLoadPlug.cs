namespace Unic.Flex.Implementation.Plugs.LoadPlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Model.Forms;
    using Model.Plugs;
    using Rules;
    using Sitecore.Rules;

    [SitecoreType(TemplateId = "{A3DF6696-BE85-4A66-8056-6773CA260742}")]
    public class RulesExecutionLoadPlug : LoadPlugBase
    {
        [SitecoreField("Rule")]
        public RuleList<RuleContext> Rule { get; set; }

        public override void Execute(IForm form)
        {
            var ruleContext = new FlexFormRuleContext();
            ruleContext.Form = form;
            foreach (Rule<RuleContext> rule in this.Rule.Rules)
            {
                if (rule.Condition == null)
                {
                    rule.Execute(ruleContext);
                }
                else
                {
                    if (rule.Evaluate(ruleContext))
                    {
                        rule.Execute(ruleContext);
                    }
                }
            }
        }
    }
}