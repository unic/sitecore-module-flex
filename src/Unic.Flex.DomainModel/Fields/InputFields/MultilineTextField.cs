using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{D5768466-2ADA-4C3C-B5C2-6B0299D08F2E}")]
    public class MultilineTextField : InputField
    {
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/MultilineText";
            }
        }

        [SitecoreField("Rows")]
        public virtual int Rows { get; set; }
    }
}
