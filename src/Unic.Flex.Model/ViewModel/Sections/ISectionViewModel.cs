namespace Unic.Flex.Model.ViewModel.Sections
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.ViewModel.Components;
    using Unic.Flex.Model.ViewModel.Fields;

    /// <summary>
    /// View model for a section
    /// </summary>
    public interface ISectionViewModel : IPresentationComponent, ITooltipViewModel
    {
        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        IList<IFieldViewModel> Fields { get; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the step title.
        /// </summary>
        /// <value>
        /// The step title.
        /// </value>
        string StepTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the fieldset markup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the fieldset markup should not be outputed; otherwise, <c>false</c>.
        /// </value>
        bool DisableFieldset { get; set; }
    }
}
