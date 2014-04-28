using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Presentation
{
    public interface IPresentationComponent
    {
        string ViewName { get; }
    }
}
