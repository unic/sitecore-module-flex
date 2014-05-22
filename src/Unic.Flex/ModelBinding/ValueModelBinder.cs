using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.ModelBinding
{
    using System.ComponentModel;
    using System.Web.Mvc;

    public class ValueModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Name == "Value")
            {
                // todo: uncomment or remove if not needed
                //propertyDescriptor.SetValue(propertyDescriptor.PropertyType, typeof(string));
            }
            
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}
