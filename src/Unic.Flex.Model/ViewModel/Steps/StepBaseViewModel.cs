namespace Unic.Flex.Model.ViewModel.Steps
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Sections;

    /// <summary>
    /// Base view model for all steps
    /// </summary>
    public abstract class StepBaseViewModel : IPresentationComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepBaseViewModel"/> class.
        /// </summary>
        protected StepBaseViewModel()
        {
            this.Sections = new List<StandardSectionViewModel>();
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public virtual IList<StandardSectionViewModel> Sections { get; private set; }
        
        /// <summary>
        /// Gets or sets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public virtual string ViewName { get; set; }
    }
}
