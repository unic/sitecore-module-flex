namespace Unic.Flex.Model.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    /// <summary>
    /// A step in a multi step form.
    /// </summary>
    [SitecoreType(TemplateId = "{93EC9DA3-81FC-4A1E-82F8-DD988A2D355B}")]
    public class MultiStep : StepBase, INavigationPane
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
        /// Gets or sets a value indicating whether this step is skippable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this step is skippable; otherwise, <c>false</c>.
        /// </value>
        [SitecoreField("Is Skippable")]
        public virtual bool IsSkippable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this step is the last step.
        /// </summary>
        /// <value>
        /// <c>true</c> if this step is the last step; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsLastStep { get; set; }
    }
}
