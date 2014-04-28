using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Fields
{
    using Unic.Flex.DomainModel.Presentation;

    public interface IFieldBase : IItemBase, IPresentationComponent
    {
        string Label { get; set; }

        string Value { get; set; }
    }
}
