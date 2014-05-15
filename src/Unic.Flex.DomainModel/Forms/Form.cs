namespace Unic.Flex.DomainModel.Forms
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.DomainModel.Steps;

    [SitecoreType(TemplateId = "{3AFE4256-1C3E-4441-98AF-B3D0037A8B1F}")]
    public class Form : ItemBase, IPresentationComponent
    {
        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        [SitecoreField("Introduction")]
        public virtual string Introduction { get; set; }

        [SitecoreQuery("./Steps/*", IsLazy = true, IsRelative = true, InferType = true)]
        public virtual IEnumerable<StepBase> Steps { get; set; }

        public virtual StepBase GetActiveStep()
        {
            return this.Steps.FirstOrDefault(step => step.IsActive) ?? this.Steps.FirstOrDefault();
        }

        public virtual string ViewName
        {
            get
            {
                return "Form";
            }
        }
    }
}
