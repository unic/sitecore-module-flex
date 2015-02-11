namespace Unic.Flex.Model.ViewModel.Steps
{
    /// <summary>
    /// View model for a single step
    /// </summary>
    public class SingleStepViewModel : StepBaseViewModel
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
                return "Steps/SingleStep";
            }
        }
    }
}
