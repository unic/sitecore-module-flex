namespace Unic.Flex.Model.ViewModel.Forms
{
    using Unic.Flex.Model.ViewModel.Steps;

    /// <summary>
    /// This view model covers the form itself.
    /// </summary>
    public class FormViewModel : IFormViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public virtual string Language { get; set; }
        
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the introduction.
        /// </summary>
        /// <value>
        /// The introduction.
        /// </value>
        public virtual string Introduction { get; set; }

        /// <summary>
        /// Gets or sets the step to render.
        /// </summary>
        /// <value>
        /// The step to render.
        /// </value>
        public virtual IStepViewModel Step { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public virtual string ViewName
        {
            get
            {
                return "Form";
            }
        }
    }
}