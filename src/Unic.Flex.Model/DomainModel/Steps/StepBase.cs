namespace Unic.Flex.Model.DomainModel.Steps
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Sections;
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
        /// Gets or sets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        public virtual int StepNumber { get; set; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        /// <value>
        /// The name of the view.
        /// </value>
        public abstract string ViewName { get; }

        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<SectionBase> Sections { get; set; }
    }
}
