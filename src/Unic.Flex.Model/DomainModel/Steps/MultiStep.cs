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
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public override string ViewName
        {
            get
            {
                return "Steps/MultiStep";
            }
        }
    }
}
