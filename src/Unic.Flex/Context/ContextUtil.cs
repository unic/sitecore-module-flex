using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Context
{
    using Unic.Flex.Model.DomainModel.Forms;

    public static class ContextUtil
    {
        public static Form GetCurrentForm()
        {
            return FlexContext.Current.Form;
        }
    }
}
