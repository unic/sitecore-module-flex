using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.DomainModel.Fields.ListFields
{
    using System.Web.Mvc;
    using Glass.Mapper.Sc.Configuration.Attributes;

    [SitecoreType(TemplateId = "{18C0BDC1-5162-4CE4-A92A-0C9A8CAFCF11}")]
    public class DropDownListField : ListField<string>
    {
        public DropDownListField()
        {
            // todo: this values should be provided from Sitecore
            
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Please Select", Value = string.Empty });
            items.Add(new SelectListItem { Text = "Male", Value = "Male" });
            items.Add(new SelectListItem { Text = "Female", Value = "Female" });
            this.Items = items;
        }
        
        public override string ViewName
        {
            get
            {
                return "Fields/ListFields/DropDownList";
            }
        }
    }
}
