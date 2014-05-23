using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.ViewModel.Steps
{
    public class MultiStepViewModel : StepBaseViewModel
    {
        public virtual string NextStepUrl { get; set; }

        public virtual string PreviousStepUrl { get; set; }
    }
}
