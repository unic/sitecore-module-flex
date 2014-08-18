using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.ViewModel.Components
{
    using Unic.Flex.Model.Presentation;

    public class ErrorViewModel : IPresentationComponent
    {
        public IEnumerable<string> Messages { get; set; } 
        
        public string ViewName
        {
            get
            {
                return "Components/Error";
            }
        }
    }
}
