namespace Unic.Flex.Model.DomainModel.Steps
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.DomainModel.Sections;

    /// <summary>
    /// A default step with sections and fields.
    /// </summary>
    public abstract class StandardStep : StepBase
    {
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
