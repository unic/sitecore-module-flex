namespace Unic.Flex.Model.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// The summary step.
    /// </summary>
    [SitecoreType(TemplateId = "{D8D5719A-4799-44D4-8BDC-9EA78E029385}")]
    public class Summary : StepBase
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Steps/Summary";
            }
        }
    }
}
