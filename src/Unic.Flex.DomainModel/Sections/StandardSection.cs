using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Sections
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DomainModel.Fields;
    using Unic.Flex.DomainModel.Presentation;

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
