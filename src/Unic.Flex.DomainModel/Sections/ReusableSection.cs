using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Sections
{
    using Glass.Mapper.Sc.Configuration.Attributes;
    using Unic.Flex.DomainModel.Fields;

    [SitecoreType(TemplateId = "{773FF85E-C512-4825-A4EA-1FFB181525C9}")]
    public class ReusableSection : SectionBase
    {
        [SitecoreField("Section")]
        public virtual StandardSection Section { get; set; }
    }
}
