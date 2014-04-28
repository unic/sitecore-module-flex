using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Sections
{
    using Unic.Flex.DomainModel.Fields;
    using Unic.Flex.DomainModel.Presentation;

    public interface ISectionBase : IItemBase, IPresentationComponent
    {
        IEnumerable<IFieldBase> Fields { get; set; } 
    }
}
