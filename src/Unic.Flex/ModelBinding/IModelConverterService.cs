using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.ModelBinding
{
    using Unic.Flex.Model.DomainModel.Forms;
    using Unic.Flex.Model.ViewModel.Forms;

    public interface IModelConverterService
    {
        FormViewModel ConvertToViewModel(Form form);
    }
}
