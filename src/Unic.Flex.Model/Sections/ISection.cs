namespace Unic.Flex.Model.Sections
{
    using System.Collections.Generic;
    using Unic.Flex.Model.Components;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// The section interface
    /// </summary>
    public interface ISection : IItemBase, IPresentationComponent, IFieldDependency, ITooltip
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }
        
        /// <summary>
        /// Gets the step title.
        /// </summary>
        /// <value>
        /// The step title.
        /// </value>
        string StepTitle { get; }

        /// <summary>
        /// Gets a value indicating whether to disable the fieldset markup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the fieldset markup should not be outputed; otherwise, <c>false</c>.
        /// </value>
        bool DisableFieldset { get; }

        /// <summary>
        /// Gets or sets whether to show a component in summary.
        /// </summary>
        /// <value>
        /// The show in summary.
        /// </value>
        bool ShowInSummary { get; set; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        IList<IField> Fields { get; }

        /// <summary>
        /// Gets the container attributes.
        /// </summary>
        /// <value>
        /// The container attributes.
        /// </value>
        IDictionary<string, object> ContainerAttributes { get; }

        /// <summary>
        /// Binds the properties.
        /// </summary>
        void BindProperties();
    }
}
