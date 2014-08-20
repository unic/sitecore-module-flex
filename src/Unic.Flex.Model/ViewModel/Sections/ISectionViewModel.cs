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
    }
}
