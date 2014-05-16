namespace Unic.Flex.Model.Steps
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Sections;

    public abstract class StandardStep : StepBase
    {
        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<SectionBase> Sections { get; set; }
    }
}
