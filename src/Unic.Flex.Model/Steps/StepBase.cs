namespace Unic.Flex.Model.Steps
{
    using Unic.Flex.Model.Presentation;

    /// <summary>
    /// Base class for all steps.
    /// </summary>
    public abstract class StepBase : ItemBase, IPresentationComponent
    {
        /// <summary>
        /// Gets or sets a value indicating whether this step is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this step is active; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public abstract string ViewName { get; }
    }
}
