namespace Unic.Flex.Model.ViewModel.Steps
{
    using Unic.Flex.Model.ViewModel.Components;

    /// <summary>
    /// View model for a summary step
    /// </summary>
    public class SummaryViewModel : StepBaseViewModel, INavigationPaneViewModel
    {
        /// <summary>
        /// Gets or sets the previous step URL.
        /// </summary>
        /// <value>
        /// The previous step URL.
        /// </value>
        public virtual string PreviousStepUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the navigation pane.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the navigation pane should be shown; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowNavigationPane { get; set; }

        /// <summary>
        /// Gets or sets the navigation pane.
        /// </summary>
        /// <value>
        /// The navigation pane.
        /// </value>
        public virtual NavigationPaneViewModel NavigationPane { get; set; }

        /// <summary>
        /// Gets or sets the introduction.
        /// </summary>
        /// <value>
        /// The introduction.
        /// </value>
        public virtual string Introduction { get; set; }

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
