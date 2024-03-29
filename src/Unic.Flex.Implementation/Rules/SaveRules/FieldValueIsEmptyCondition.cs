﻿namespace Unic.Flex.Implementation.Rules.SaveRules
{
    using Core.Rules;
    using Model.Forms;
    using Sitecore.Diagnostics;
    using Sitecore.Rules;
    using Sitecore.Rules.Conditions;
    using System.Linq;

    public class FieldValueIsEmptyCondition<T> : WhenCondition<T> where T : RuleContext
    {
        public string FieldKey { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var fieldValue = this.GetFieldValue(ruleContext);

            return string.IsNullOrEmpty(fieldValue);
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
