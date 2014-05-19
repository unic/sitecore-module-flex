using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Mapping
{
    public interface IUserDataRepository
    {
        object GetValue(string formId, string fieldId);

        void SetValue(string formId, string fieldId, object value);
    }
}
