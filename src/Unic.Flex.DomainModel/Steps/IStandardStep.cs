using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Steps
{
    using Unic.Flex.DomainModel.Sections;

    public interface IStandardStep : IStepBase
    {
        IEnumerable<ISectionBase> Sections { get; set; } 
    }
}
