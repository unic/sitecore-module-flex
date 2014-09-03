namespace Unic.Flex.Model.ViewModel.Forms
{
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Steps;

    /// <summary>
    /// Inteface for the form view model
    /// </summary>
    public interface IFormViewModel : IPresentationComponent
    {
        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        IStepViewModel Step { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the form.
        /// </summary>
        /// <value>
        /// The form identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        string Language { get; set; }
    }
}
