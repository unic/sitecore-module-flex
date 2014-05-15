using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Steps
{
    using Unic.Flex.DomainModel.Presentation;

    public abstract class StepBase : ItemBase, IPresentationComponent
    {
        public virtual bool IsActive { get; set; }

        public abstract string ViewName { get; }
    }
}
