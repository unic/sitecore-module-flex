namespace Unic.Flex.Model.Plugs
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Sitecore.Rules;
    using System;
    using System.Linq;
    using Unic.Flex.Model.Forms;

    /// <summary>
    /// Base class for all save plugs
    /// </summary>
    [SitecoreType(TemplateId = "{40AEA426-7971-40B9-B997-339DCFFEE58A}")]
    public abstract class SavePlugBase : ISavePlug
    {
        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [SitecoreId]
        public virtual Guid ItemId { get; set; }
        
        /// <summary>
        /// Gets a value indicating whether this plug should be executed asynchronous.
        /// </summary>
        /// <value>
        /// <c>true</c> if this plug should be executed asynchronous; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsAsync { get; }

        [SitecoreField("Conditional Rule")]
        public RuleList<RuleContext> ConditionalRule { get; set; }

        /// <summary>
        /// Check if the save plug can be executed.
        /// </summary>
        /// <c>true</c> if all conditions are met; otherwise, <c>false</c>.
        /// <param name="form"></param>
        public bool CanExecute(IForm form)
        {
            var ruleContext = new FlexFormRuleContext();
            ruleContext.Form = form;

            return !this.ConditionalRule.Rules.Any() || this.ConditionalRule.Rules.All(rule => rule.Evaluate(ruleContext));
        }

        /// <summary>
        /// Executes the save plug.
        /// </summary>
        /// <param name="form">The form.</param>
        public abstract void Execute(IForm form);
    }
}
