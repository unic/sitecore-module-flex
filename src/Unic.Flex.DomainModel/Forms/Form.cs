namespace Unic.Flex.DomainModel.Forms
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DomainModel.Steps;

    [SitecoreType(TemplateId = "{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}")]
    public class Form : ItemBase, IForm
    {
        [SitecoreQuery("./Steps/*", IsLazy = true, IsRelative = true, InferType = true)]
        public virtual IEnumerable<IStepBase> Steps { get; set; }
    }
}
