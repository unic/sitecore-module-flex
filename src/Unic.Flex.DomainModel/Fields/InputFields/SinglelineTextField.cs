using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Fields.InputFields
{
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{C706D294-ADC2-477F-B5E1-63EB1EF345E5}")]
    public class SinglelineTextField : InputField
    {
        public override string ViewName
        {
            get
            {
                return "Fields/InputFields/SinglelineText";
            }
        }
    }
}
