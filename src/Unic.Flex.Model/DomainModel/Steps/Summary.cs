namespace Unic.Flex.Model.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// The summary step.
    /// </summary>
    [SitecoreType(TemplateId = "{D8D5719A-4799-44D4-8BDC-9EA78E029385}")]
    public class Summary : StepBase, INavigationPane
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the navigation pane.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the navigation pane should be shown; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Show Navigation Pane")]
        public virtual bool ShowNavigationPane { get; set; }
    }
}
