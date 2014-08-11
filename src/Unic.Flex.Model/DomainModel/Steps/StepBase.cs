namespace Unic.Flex.Model.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using System.Collections.Generic;
    using Unic.Flex.Model.DomainModel.Sections;

    /// <summary>
    /// Base class for all steps.
    /// </summary>
    public abstract class StepBase : ItemBase
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
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [SitecoreField("Title")]
        public virtual string Title { get; set; }

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
