namespace Unic.Flex.Model.Steps
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Sections;

    /// <summary>
    /// The step interface
    /// </summary>
    public interface IStep : IItemBase, IPresentationComponent
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this step is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this step is active; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        int StepNumber { get; set; }

        /// <summary>
        /// Gets or sets the cancel URL.
        /// </summary>
        /// <value>
        /// The cancel URL.
        /// </value>
        string CancelUrl { get; set; }

        /// <summary>
        /// Gets or sets the cancel text.
        /// </summary>
        /// <value>
        /// The cancel text.
        /// </value>
        string CancelText { get; set; }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        IList<StandardSection> Sections { get; }
    }
}
