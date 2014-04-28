using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Steps
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{D8D5719A-4799-44D4-8BDC-9EA78E029385}")]
    public class Summary : ItemBase, IStepBase
    {
        public virtual string ViewName
        {
            get
            {
                return "Steps/Summary";
            }
        }

        public bool IsActive { get; set; }
    }
}
