using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.ViewModel.Components
{
    public class NavigationItem
    {
        public virtual string Title { get; set; }

        public virtual string Url { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual bool IsLinked { get; set; }
    }
}
