namespace Unic.Flex.Rules.Conditions
{
    using Sitecore.Diagnostics;
    using Sitecore.Rules;
    using Sitecore.Rules.Conditions;

    /// <summary>
    /// Rules engine condition to verify the current site's instance name.
    /// </summary>
    /// <typeparam name="T">Generic parameter</typeparam>
    public class InstanceCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        /// <summary>
        /// The instance name property
        /// </summary>
        private const string InstanceNameProperty = "instanceName";

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Executes the specified rule context.
        /// </summary>
        /// <param name="ruleContext">The rule context.</param>
        /// <returns>Boolean value if the current instance matches the condition</returns>
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            if (this.Value == null) return false;

            var instanceName = Sitecore.Context.Site.Properties[InstanceNameProperty];
            return this.Compare(instanceName, this.Value);
        }
    }
}
