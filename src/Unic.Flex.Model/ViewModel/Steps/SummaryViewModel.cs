namespace Unic.Flex.Model.ViewModel.Steps
{
    /// <summary>
    /// View model for a summary step
    /// </summary>
    public class SummaryViewModel : StepBaseViewModel
    {
        /// <summary>
        /// Gets or sets the previous step URL.
        /// </summary>
        /// <value>
        /// The previous step URL.
        /// </value>
        public virtual string PreviousStepUrl { get; set; }
    }
}
