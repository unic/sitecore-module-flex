namespace Unic.Flex.Model.ViewModel.Steps
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Sections;

    /// <summary>
    /// Base view model for all steps
    /// </summary>
    public abstract class StepBaseViewModel : IStepViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepBaseViewModel"/> class.
        /// </summary>
        protected StepBaseViewModel()
        {
            this.Sections = new List<ISectionViewModel>();
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public virtual IList<ISectionViewModel> Sections { get; private set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public abstract string ViewName { get; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the cancel URL.
        /// </summary>
        /// <value>
        /// The cancel URL.
        /// </value>
        public virtual string CancelUrl { get; set; }

        /// <summary>
        /// Gets or sets the cancel text.
        /// </summary>
        /// <value>
        /// The cancel text.
        /// </value>
        public virtual string CancelText { get; set; }
    }
}
