using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.Validators
{
    public interface IValidator
    {
        bool IsValid(object value);

        IDictionary<string, object> GetAttributes();

        string ValidationMessage { get; set; }
    }
}
