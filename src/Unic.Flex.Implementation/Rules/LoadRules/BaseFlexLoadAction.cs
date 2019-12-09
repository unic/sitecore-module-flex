namespace Unic.Flex.Implementation.Rules.LoadRules
{
    using Core.Rules.Conditions;
    using Model.Forms;
    using Sitecore.Rules;
    using Sitecore.Rules.Actions;
    using System.Linq;

    public abstract class BaseFlexLoadAction<T> : RuleAction<T>
        where T : RuleContext
    {
        public string FieldKey { get; set; }

        public override void Apply(T ruleContext)
        {
            var flexContext = ruleContext as FlexFormRuleContext;
            var form = flexContext?.Form;

            var field = form?.GetFields().FirstOrDefault(_ => _.Key == this.FieldKey);

            if (field == null) return;

            field.Value = this.GetValue();
        }

        protected abstract object GetValue();
    }
}
