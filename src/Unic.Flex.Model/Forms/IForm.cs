namespace Unic.Flex.Model.Forms
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Fields;
    using Unic.Flex.Model.Plugs;
    using Unic.Flex.Model.Presentation;
    using Unic.Flex.Model.Steps;

    /// <summary>
    /// The form interface
    /// </summary>
    public interface IForm : IItemBase, IPresentationComponent
    {
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        string Language { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }

        /// <summary>
        /// Gets the cancel link.
        /// </summary>
        /// <value>
        /// The cancel link.
        /// </value>
        Link CancelLink { get; }

        /// <summary>
        /// Gets the success message.
        /// </summary>
        /// <value>
        /// The success message.
        /// </value>
        string SuccessMessage { get; }

        /// <summary>
        /// Gets the success redirect.
        /// </summary>
        /// <value>
        /// The success redirect.
        /// </value>
        ItemBase SuccessRedirect { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        string ErrorMessage { get; }

        /// <summary>
        /// Gets the steps.
        /// </summary>
        /// <value>
        /// The steps.
        /// </value>
        IEnumerable<StepBase> Steps { get; }

        /// <summary>
        /// Gets the active step.
        /// </summary>
        /// <value>
        /// The active step.
        /// </value>
        StepBase ActiveStep { get; }

        /// <summary>
        /// Gets the load plugs.
        /// </summary>
        /// <value>
        /// The load plugs.
        /// </value>
        IEnumerable<ILoadPlug> LoadPlugs { get; }

        /// <summary>
        /// Gets the save plugs.
        /// </summary>
        /// <value>
        /// The save plugs.
        /// </value>
        IEnumerable<ISavePlug> SavePlugs { get; }
    }
}
