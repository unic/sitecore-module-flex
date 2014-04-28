using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.DomainModel.Forms
{
    using Unic.Flex.DomainModel.Presentation;
    using Unic.Flex.DomainModel.Steps;

    public interface IForm : IItemBase, IPresentationComponent
    {
        string Title { get; set; }

        string Introduction { get; set; }
        
        IEnumerable<IStepBase> Steps { get; set; }

        IStepBase GetActiveStep();
    }
}
