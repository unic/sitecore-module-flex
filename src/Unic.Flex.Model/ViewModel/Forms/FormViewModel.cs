namespace Unic.Flex.Model.ViewModel.Forms
{
    using Unic.Flex.Model.ViewModel.Steps;

    /// <summary>
    /// This view model covers the form itself.
    /// </summary>
    public class FormViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the introduction.
        /// </summary>
        /// <value>
        /// The introduction.
        /// </value>
        public string Introduction { get; set; }

        /// <summary>
        /// Gets or sets the step to render.
        /// </summary>
        /// <value>
        /// The step to render.
        /// </value>
        public StepViewModel Step { get; set; }
    }
}