namespace Unic.Flex.Rules.Conditions
{
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Rules;
    using Sitecore.Rules.Conditions;
    using Unic.Flex.Context;

    /// <summary>
    /// Conditional rule to validate the current form.
    /// </summary>
    /// <typeparam name="T">Generic parameter</typeparam>
    public class FormCondition<T> : WhenCondition<T> where T : RuleContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormCondition{T}"/> class.
        /// </summary>
        public FormCondition()
        {
            this.ItemId = ID.Null;
        }

        /// <summary>
        /// Gets or sets the item id.
        /// </summary>
        /// <value>
        /// The item id.
        /// </value>
        public ID ItemId { get; set; }

        /// <summary>
        /// Executes the specified rule context.
        /// </summary>
        /// <param name="ruleContext">The rule context.</param>
        /// <returns>Boolean value wheater the condition is true/false</returns>
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            // get the current form
            var formContext = FlexContext.Current.Form;
            if (formContext == null) return false;

            // compare the id's
            return new ID(formContext.Id) == this.ItemId;
        }
    }
}
