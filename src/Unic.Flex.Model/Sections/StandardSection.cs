namespace Unic.Flex.Model.Sections
{
    using System.Collections.Generic;
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.Model.Fields;
    using Unic.Flex.Model.Presentation;

    [SitecoreType(TemplateId = "{B2B5CAB2-2BD7-4FFE-80B1-7607A310771E}")]
    public class StandardSection : SectionBase, IPresentationComponent
    {
        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        [SitecoreField("Disable Fieldset")]
        public virtual bool DisableFieldset { get; set; }
        
        public virtual string ViewName
        {
            get
            {
                return "Sections/StandardSection";
            }
        }

        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<FieldBase> Fields { get; set; }
    }
}
