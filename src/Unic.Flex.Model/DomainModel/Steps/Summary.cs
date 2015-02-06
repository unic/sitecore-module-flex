namespace Unic.Flex.Model.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Components;
    using Unic.Flex.Model.GlassExtensions.Attributes;

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

        /// <summary>
        /// Gets or sets the previous button text.
        /// </summary>
        /// <value>
        /// The previous button text.
        /// </value>
        [SitecoreDictionaryFallbackField("Previous Button Text", "Previous Step")]
        public virtual string PreviousButtonText { get; set; }
    }
}
