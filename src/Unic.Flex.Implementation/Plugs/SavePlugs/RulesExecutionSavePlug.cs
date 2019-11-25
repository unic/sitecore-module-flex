namespace Unic.Flex.Implementation.Plugs.SavePlugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Model.Forms;
    using Model.Plugs;
    using Rules;
    using Sitecore.Rules;

    [SitecoreType(TemplateId = "{03B67200-F3F4-49C9-ADC5-31512371825D}")]
    public class RulesExecutionSavePlug : SavePlugBase
    {
        [SitecoreField("Rule")]
        public RuleList<RuleContext> Rule { get; set; }

        public override bool IsAsync { get; } = false;

        public override void Execute(IForm form)
        {
            if (!this.IsConditionFulfilled(form)) return;

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