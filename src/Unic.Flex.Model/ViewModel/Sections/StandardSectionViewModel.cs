namespace Unic.Flex.Model.ViewModel.Sections
{
    using System.Collections.Generic;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// This view model covers a section in the form step.
    /// </summary>
    public class StandardSectionViewModel : ISectionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardSectionViewModel" /> class.
        /// </summary>
        public StandardSectionViewModel()
        {
            this.Fields = new List<IFieldViewModel>();
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public virtual IList<IFieldViewModel> Fields { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the fieldset markup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the fieldset markup should not be outputed; otherwise, <c>false</c>.
        /// </value>
        public virtual bool DisableFieldset { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the step title.
        /// </summary>
        /// <value>
        /// The step title.
        /// </value>
        public virtual string StepTitle { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public virtual TooltipViewModel Tooltip { get; set; }

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
                return "Sections/StandardSection";
            }
        }
    }
}