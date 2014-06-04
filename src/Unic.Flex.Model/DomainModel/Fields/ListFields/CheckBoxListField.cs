using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{7532F52A-8BA7-4903-9B27-9E18FA1C4B92}")]
    public class CheckBoxListField : ListField<string[]>
    {
        public CheckBoxListField()
        {
            // todo: this values should be provided from Sitecore

            this.Items.Add(new SelectListItem { Text = "Red", Value = "red" });
            this.Items.Add(new SelectListItem { Text = "Green", Value = "green" });
            this.Items.Add(new SelectListItem { Text = "Blue", Value = "blue" });
        }
        
        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/CheckBoxList";
            }
        }
    }
}
