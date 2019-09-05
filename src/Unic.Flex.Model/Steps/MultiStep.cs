namespace Unic.Flex.Model.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.GlassExtensions.Attributes;

    /// <summary>
    /// A step in a multi step form.
    /// </summary>
    [SitecoreType(TemplateId = "{93EC9DA3-81FC-4A1E-82F8-DD988A2D355B}")]
    public class MultiStep : StepBase
    {
        /// <summary>
        /// Gets or sets CSS classes for title.
        /// </summary>
        /// <value>
        /// The Header Additional Classes field.
        /// </value>
        [SitecoreField("Header Additional Classes")]
        public virtual string HeaderAdditionalClasses { get; set; }

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
        /// Gets or sets the previous button text.
        /// </summary>
        /// <value>
        /// The previous button text.
        /// </value>
        [SitecoreDictionaryFallbackField("Previous Button Text", "Previous Step")]
        public virtual string PreviousButtonText { get; set; }

        /// <summary>
        /// Gets or sets the skip button text.
        /// </summary>
        /// <value>
        /// The skip button text.
        /// </value>
        [SitecoreDictionaryFallbackField("Skip Button Text", "Skip Step")]
        public virtual string SkipButtonText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this step is the last step.
        /// </summary>
        /// <value>
        /// <c>true</c> if this step is the last step; otherwise, <c>false</c>.
        /// </value>
        [SitecoreIgnore]
        public virtual bool IsLastStep { get; set; }

        /// <summary>
        /// Gets or sets the previous step URL.
        /// </summary>
        /// <value>
        /// The previous step URL.
        /// </value>
        [SitecoreIgnore]
        public virtual string PreviousStepUrl { get; set; }

        /// <summary>
        /// Gets or sets the next step URL.
        /// </summary>
        /// <value>
        /// The next step URL.
        /// </value>
        [SitecoreIgnore]
        public virtual string NextStepUrl { get; set; }

        /// <summary>
        /// Gets or sets the navigation pane.
        /// </summary>
        /// <value>
        /// The navigation pane.
        /// </value>
        [SitecoreIgnore]
        public virtual NavigationPane NavigationPane { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        [SitecoreIgnore]
        public override string ViewName
        {
            get
            {
                return "Steps/MultiStep";
            }
        }
    }
}
