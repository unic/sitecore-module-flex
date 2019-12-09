namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.Rules.Conditions;
    using Model.Forms;
    using Sitecore.Diagnostics;
    using Sitecore.Rules;
    using Sitecore.Rules.Conditions;
    using System.Linq;

    public class FieldValueCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string FieldKey { get; set; }
        
        public string Value { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var fieldValue = this.GetFieldValue(ruleContext);

            return this.Compare(fieldValue, this.Value);
        }


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
