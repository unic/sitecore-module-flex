namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.Rules;
    using Model.Forms;
    using Sitecore.Rules;
    using Sitecore.Rules.Actions;
    using System.Linq;

    public abstract class BaseFlexSaveAction<T> : RuleAction<T> where T : RuleContext
    {
        public string FieldKey { get; set; }

        protected string GetFieldValue(T ruleContext)
        {
            var flexContext = ruleContext as FlexFormRuleContext;
            var form = flexContext?.Form;

            var field = form?.GetFields().FirstOrDefault(_ => _.Key == this.FieldKey);
            if (field?.Value == null) return null;

            var value = field.Value.ToString();
            return value;
        }
    }
}