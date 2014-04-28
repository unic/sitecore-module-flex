using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Steps
{
    using Unic.Flex.DomainModel.Presentation;

    public interface IStepBase : IItemBase, IPresentationComponent
    {
        bool IsActive { get; set; }
    }
}
