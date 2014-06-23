namespace Unic.Flex.Model.ViewModel.Steps
{
    /// <summary>
    /// View model for a step in a multi step form
    /// </summary>
    public class MultiStepViewModel : StepBaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the navigation pane.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the navigation pane should be shown; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowNavigationPane { get; set; }
        
        /// <summary>
        /// Gets or sets the next step URL.
        /// </summary>
        /// <value>
        /// The next step URL.
        /// </value>
        public virtual string NextStepUrl { get; set; }

        /// <summary>
        /// Gets or sets the previous step URL.
        /// </summary>
        /// <value>
        /// The previous step URL.
        /// </value>
        public virtual string PreviousStepUrl { get; set; }
    }
}
